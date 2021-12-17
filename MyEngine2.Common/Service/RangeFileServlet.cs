using MyEngine2.Common.Net;

namespace MyEngine2.Common.Service
{
    /// <summary>
    /// 支持断点续传的文件 Servlet
    /// </summary>
    public class RangeFileServlet : FileServlet
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="profile">服务器配置文件</param>
        /// <param name="keepAlive">是否为长连接 - 仅做字段</param>
        public RangeFileServlet(ServiceProfile.ServerProfile profile, bool keepAlive) : base(profile, keepAlive)
        {
        }

        /// <summary>
        /// 初始化通用响应
        /// </summary>
        /// <returns>初始化响应头</returns>
        protected override HttpResponse InitResponse()
        {
            var response = new HttpResponse();
            response.SetHeader("Date", DateTime.Now.ToString("R"));
            response.SetHeader("Server", Profile.Name);
            response.SetHeader("Connection", KeepAlive ? "KeepAlive" : "Close");
            response.SetHeader("Accept-Ranges", "Bytes");
            return response;
        }

        /// <summary>
        /// 请求区间格式错误响应 - 416
        /// </summary>
        /// <param name="socket"></param>
        protected void SendRangesInvalidFormat(BaseSocket socket)
        {
            HttpResponse response = InitResponse();
            response.AutoConfiguration("416");
        }

        /// <summary>
        /// 请求区间越界响应 - 416
        /// </summary>
        /// <param name="socket"></param>
        protected void SendRequestedRangeNotSatisfiable(BaseSocket socket)
        {
            HttpResponse response = InitResponse();
            response.AutoConfiguration("416");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="request"></param>
        public override void OnGet(BaseSocket socket, HttpRequest request)
        {
            string rangeString = request.GetHeader("Range");
            // 完整文件请求
            if (rangeString.Length == 0)
            {
                base.OnGet(socket, request);
            }
            // 分块文件请求
            else
            {
                string realPath = Profile.Root + request.Url;
                // 校验路径
                if (!(File.Exists(realPath) && AssertPath(realPath)))
                {
                    LoggerManager.Logger.Warn(string.Format("Request File {0} Does Not Exists", request.Url));
                    SendNotFound(socket);
                    return;
                }

                FileInfo fileInfo = new(realPath);
                long contentLength = 0;
                LinkedList<HttpRange> ranges;
                // 校验区间
                try
                {
                    HttpRangeParser parser = new(rangeString);
                    ranges = parser.HttpRanges;
                    if (ranges.Count == 0)
                    {
                        LoggerManager.Logger.Warn("Ranges Count Equal Zero");
                        SendRangesInvalidFormat(socket);
                        return;
                    }
                }
                catch (Exception)
                {
                    LoggerManager.Logger.Warn("Ranges Invalid Format");
                    SendRangesInvalidFormat(socket);
                    return;
                }

                // 重定义区间
                foreach (var range in ranges)
                {
                    if (range.Start < 0) range.Start = 0;
                    if (range.End < 0) range.End = fileInfo.Length - 1;

                    if (range.Start > range.End || range.End > fileInfo.Length)
                    {
                        SendRequestedRangeNotSatisfiable(socket);
                        return;
                    }

                    contentLength += range.End - range.Start + 1;
                }

                // 回应文件块
                bool isFirst = true;
                using FileStream fileStream = new(realPath, FileMode.Open, FileAccess.Read);
                foreach (var range in ranges)
                {
                    LoggerManager.Logger.Info(
                        string.Format(
                            "Request File Block {0} [{1}, {2}]",
                            request.Url,
                            range.Start,
                            range.End)
                        );
                    long sendCount = range.End - range.Start + 1;
                    if (isFirst)
                    {
                        HttpResponse response = InitResponse();
                        response.AutoConfiguration("206");
                        response.ContentLength = contentLength.ToString();
                        SendResponse(socket, response);
                        isFirst = false;
                    }
                    else
                    {
                        string subheader = string.Format("Content-Range: {0}", range.ToString(fileInfo.Length));
                        socket.WriteLine(subheader);
                    }

                    fileStream.Seek(range.Start, SeekOrigin.Begin);
                    byte[] buffer = new byte[BufferSize];
                    while (sendCount > 0)
                    {
                        if (sendCount > BufferSize)
                        {
                            fileStream.Read(buffer, 0, BufferSize);
                            var l = socket.Send(buffer, BufferSize, System.Net.Sockets.SocketFlags.None);
                            if (l == -1)
                            {
                                break;
                            }
                            sendCount -= l;
                        }
                        else
                        {
                            fileStream.Read(buffer, 0, (int)sendCount);
                            var l = socket.Send(buffer, (int)sendCount, System.Net.Sockets.SocketFlags.None);
                            if (l == -1)
                            {
                                break;
                            }
                            sendCount -= l;
                        }
                        Thread.Sleep(0);
                    }
                }
            }
        }
    }
}