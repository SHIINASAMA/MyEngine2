using MyEngine2.Common.Net;

namespace MyEngine2.Common.Service
{
    /// <summary>
    /// 普通文件 Servlet
    /// </summary>
    public class FileServlet : BaseServlet
    {
        // Socket 单次发送缓存大小
        protected static readonly int BufferSize = 1024;

        // 是否为长连接
        protected bool KeepAlive;

        // 服务器配置文件
        protected ServiceProfile.ServerProfile Profile;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="root">根目录</param>
        /// <param name="homePage">自定义主页</param>
        /// <param name="notFoundPage">自定义 404 页面</param>
        public FileServlet(ServiceProfile.ServerProfile profile, bool keepAlive) : base()
        {
            Profile = profile;
            KeepAlive = keepAlive;
        }

        /// <summary>
        /// 判断路径是否返回上一级
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>返回上一级返回 True， 否则为 False</returns>
        protected bool AssertPath(string path)
        {
            return !path.Contains("..");
        }

        /// <summary>
        /// 比较两个文件相对路径是否相同
        /// </summary>
        /// <param name="path1">左值</param>
        /// <param name="path2">右值</param>
        /// <returns>相同返回 True，否则为 False</returns>
        protected bool ComparePath(string path1, string path2)
        {
            return Path.GetRelativePath(path1, ".") == Path.GetRelativePath(path2, ".");
        }

        /// <summary>
        /// 初始化通用响应
        /// </summary>
        /// <returns>初始化响应头</returns>
        protected virtual HttpResponse InitResponse()
        {
            var response = new HttpResponse();
            response.SetHeader("Date", DateTime.Now.ToString("R"));
            response.SetHeader("Server", Profile.Name);
            response.SetHeader("Accept-Ranges", "None");
            return response;
        }

        protected override void SendResponse(SocketContext context, HttpResponse response)
        {
            HttpHelper httpHelper = new(context.Socket);
            response.SetHeader("Connection", KeepAlive && context.KeepAlive ? "Keep-Alive" : "Close");
            httpHelper.WriteResponse(response);
            // 请求次数加 1
            context.Requested();
        }

        /// <summary>
        /// 发送完整文件
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="path"></param>
        protected void SendFile(BaseSocket socket, string path)
        {
            using FileStream fileStream = new(path, FileMode.Open, FileAccess.Read);
            int length;
            byte[] buffer = new byte[BufferSize];
            while ((length = fileStream.Read(buffer, 0, BufferSize)) > 0)
            {
                socket.Send(buffer, length, System.Net.Sockets.SocketFlags.None);
            }
        }

        /// <summary>
        /// 发送 404 报文
        /// </summary>
        /// <param name="context">客户端 Socket 上下文</param>
        protected void SendNotFound(SocketContext context)
        {
            if (Profile.NotFoundPage.Enable)
            {
                string realPath = Profile.Root + Profile.NotFoundPage.Path;
                if (File.Exists(realPath))
                {
                    LoggerManager.Logger.Info(string.Format("Send Custom 404 Page {0}", Profile.NotFoundPage.Path));
                    HttpResponse response = InitResponse();
                    response.AutoConfiguration("404");
                    response.ContentLength = new FileInfo(realPath).Length.ToString();
                    SendResponse(context, response);
                    SendFile(context.Socket, realPath);
                }
                else
                {
                    LoggerManager.Logger.Warn(string.Format("404 Page {0} Does Not Exists, Use Default Response", Profile.NotFoundPage.Path));
                    HttpResponse response = InitResponse();
                    response.AutoConfiguration("404");
                    SendResponse(context, response);
                }
            }
            else
            {
                LoggerManager.Logger.Info("Send 404 Response");
                HttpResponse response = InitResponse();
                response.AutoConfiguration("404");
                SendResponse(context, response);
            }
        }

        /// <summary>
        /// 请求整个文件
        /// </summary>
        /// <param name="context">客户端 Socket 上下文</param>
        /// <param name="request">请求</param>
        public override void OnGet(SocketContext context, HttpRequest request)
        {
            string realPath = Profile.Root + request.Url;

            // 请求路径为根路径 并且 设置了主页面
            if (ComparePath(Profile.Root, realPath) && Profile.HomePage.Enable)
            {
                string homePagePath = Profile.Root + Profile.HomePage.Path;
                if (File.Exists(homePagePath))
                {
                    LoggerManager.Logger.Info(string.Format("Request Custom Home Page {0} ", Profile.HomePage.Path));
                    HttpResponse response = InitResponse();
                    response.AutoConfiguration("200");
                    response.ContentLength = new FileInfo(homePagePath).Length.ToString();
                    SendResponse(context, response);
                    SendFile(context.Socket, homePagePath);
                }
                else
                {
                    LoggerManager.Logger.Warn(string.Format("Request Custom Home Page '{0}' Does Not Exists", Profile.HomePage.Path));
                    SendNotFound(context);
                }
            }
            // 请求普通文件
            else if (File.Exists(realPath) && AssertPath(realPath))
            {
                LoggerManager.Logger.Info(string.Format("Request File {0}", request.Url));
                HttpResponse response = InitResponse();
                response.AutoConfiguration("200");
                response.ContentLength = new FileInfo(realPath).Length.ToString();
                SendResponse(context, response);
                SendFile(context.Socket, realPath);
            }
            // 404
            else
            {
                LoggerManager.Logger.Warn(string.Format("Request File {0} Does Not Exists", request.Url));
                SendNotFound(context);
            }
        }
    }
}