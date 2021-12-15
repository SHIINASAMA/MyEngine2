using MyEngine2.Common.Net;

namespace MyEngine2.Common.Service
{
    /// <summary>
    /// 普通文件 Servlet
    /// </summary>
    public class FileServlet : BaseServlet
    {
        // 根目录
        private string Root;

        // Socket 单次发送缓存大小
        protected static readonly int BufferSize = 1024;

        /// <summary>
        /// 构造函数
        /// </summary>
        public FileServlet(string root) : base()
        {
            Root = root;
        }

        /// <summary>
        /// 判断路径是否返回上一级
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        protected bool AssertPath(string path)
        {
            return !path.Contains("..");
        }

        /// <summary>
        /// 请求整个文件
        /// </summary>
        /// <param name="socket">客户端 Socket</param>
        /// <param name="request">请求</param>
        public override void OnGet(BaseSocket socket, HttpRequest request)
        {
            string realPath = Root + request.Url;
            HttpResponse response = new();
            if (File.Exists(realPath) && AssertPath(realPath))
            {
                LoggerManager.Logger.Info(string.Format("Request File {0}", realPath));
                response.AutoConfiguration("200");
                response.ContentLength = new FileInfo(realPath).Length.ToString();
                SendResponse(socket, response);

                using FileStream fileStream = new(realPath, FileMode.Open, FileAccess.Read);
                int length;
                byte[] buffer = new byte[BufferSize];
                while ((length = fileStream.Read(buffer, 0, BufferSize)) > 0)
                {
                    socket.Send(buffer, length, System.Net.Sockets.SocketFlags.None);
                }
            }
            else
            {
                response.AutoConfiguration("404");
                SendResponse(socket, response);
            }
        }
    }
}