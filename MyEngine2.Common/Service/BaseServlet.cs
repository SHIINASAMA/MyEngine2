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
        /// <param name="socket">客户端 Socket</param>
        public void Exec(BaseSocket socket)
        {
            HttpHelper httpHelper = new(socket);
            HttpRequest? request = httpHelper.ReadRequest();
            if (request == null)
            {
                throw new ServletException("HttpRequest 为空");
            }
            OnRequest(socket, request);
        }

        /// <summary>
        /// 发送响应
        /// </summary>
        /// <param name="socket">客户端 Socket</param>
        /// <param name="response">响应</param>
        public void SendResponse(BaseSocket socket, HttpResponse response)
        {
            HttpHelper httpHelper = new(socket);
            httpHelper.WriteResponse(response);
        }

        private void OnRequest(BaseSocket socket, HttpRequest request)
        {
            switch (request.Method)
            {
                case Net.HttpMethod.Get:
                    OnGet(socket, request);
                    break;

                case Net.HttpMethod.Post:
                    OnPost(socket, request);
                    break;

                case Net.HttpMethod.Nonsupport:
                    OnNonsupport(socket, request);
                    break;
            }
        }

        /// <summary>
        /// 当请求方法为 GET 时执行，默认返回 405
        /// </summary>
        /// <param name="socket">客户端套接字</param>
        /// <param name="request">请求</param>
        public virtual void OnGet(BaseSocket socket, HttpRequest request)
        {
            HttpResponse response = new();
            response.StateCode = "405";
            response.Description = HttpState.Description["405"];
            SendResponse(socket, response);
        }

        /// <summary>
        /// 当请求方法为 POST 时执行，默认返回 405
        /// </summary>
        /// <param name="socket">客户端套接字</param>
        /// <param name="request">请求</param>
        public virtual void OnPost(BaseSocket socket, HttpRequest request)
        {
            HttpResponse response = new();
            response.StateCode = "405";
            response.Description = HttpState.Description["405"];
            SendResponse(socket, response);
        }

        /// <summary>
        /// 当请求方法为 Nonsupport 时执行，默认返回 501
        /// </summary>
        /// <param name="socket">客户端套接字</param>
        /// <param name="request">请求</param>
        public virtual void OnNonsupport(BaseSocket socket, HttpRequest request)
        {
            HttpResponse response = new();
            response.StateCode = "501";
            response.Description = HttpState.Description["501"];
            SendResponse(socket, response);
        }
    }
}