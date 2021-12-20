using MyEngine2.Common.Net;

namespace MyEngine2.Common.Service
{
    /// <summary>
    /// 基础 Servlet
    /// </summary>
    public class BaseServlet
    {
        /// <summary>
        /// Servlet Url
        /// </summary>
        public string Url { get; set; } = "/";

        /// <summary>
        /// 构造函数
        /// </summary>
        public BaseServlet()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="url">注册 Url</param>
        public BaseServlet(string url)
        {
            Url = url;
        }

        /// <summary>
        /// 调用服务
        /// </summary>
        /// <param name="context">客户端 Socket 上下文</param>
        public void Exec(SocketContext context)
        {
            HttpHelper httpHelper = new(context.Socket);
            HttpRequest? request = httpHelper.ReadRequest();
            if (request == null)
            {
                throw new ServletException("HttpRequest 为空");
            }

            // 若对方没有设置 KeepAlive 则之后关闭链接
            if (request.GetHeader("Connection").ToUpper() == "KEEP-ALIVE")
            {
                context.KeepAlive = true;
            }
            else
            {
                context.KeepAlive = false;
            }

            OnRequest(context, request);
        }

        /// <summary>
        /// 发送响应
        /// </summary>
        /// <param name="context">客户端 Socket 上下文</param>
        /// <param name="response">响应</param>
        protected virtual void SendResponse(SocketContext context, HttpResponse response)
        {
            HttpHelper httpHelper = new(context.Socket);
            httpHelper.WriteResponse(response);
            // 请求次数加 1
            context.Requested();
        }

        private void OnRequest(SocketContext context, HttpRequest request)
        {
            switch (request.Method)
            {
                case Net.HttpMethod.Get:
                    OnGet(context, request);
                    break;

                case Net.HttpMethod.Post:
                    OnPost(context, request);
                    break;

                case Net.HttpMethod.Nonsupport:
                    OnNonsupport(context, request);
                    break;
            }
        }

        /// <summary>
        /// 当请求方法为 GET 时执行，默认返回 405
        /// </summary>
        /// <param name="context">客户端套接字</param>
        /// <param name="request">请求</param>
        public virtual void OnGet(SocketContext context, HttpRequest request)
        {
            HttpResponse response = new();
            response.StateCode = "405";
            response.Description = HttpState.Description["405"];
            SendResponse(context, response);
        }

        /// <summary>
        /// 当请求方法为 POST 时执行，默认返回 405
        /// </summary>
        /// <param name="context">客户端套接字</param>
        /// <param name="request">请求</param>
        public virtual void OnPost(SocketContext context, HttpRequest request)
        {
            HttpResponse response = new();
            response.StateCode = "405";
            response.Description = HttpState.Description["405"];
            SendResponse(context, response);
        }

        /// <summary>
        /// 当请求方法为 Nonsupport 时执行，默认返回 501
        /// </summary>
        /// <param name="context">客户端套接字</param>
        /// <param name="request">请求</param>
        public virtual void OnNonsupport(SocketContext context, HttpRequest request)
        {
            HttpResponse response = new();
            response.StateCode = "501";
            response.Description = HttpState.Description["501"];
            SendResponse(context, response);
        }
    }
}