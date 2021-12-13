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
        public string Url { get; set; }

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

            HttpResponse response = new HttpResponse();
            OnRequest(request, response);
            httpHelper.WriteResponse(response);
            AfterRequest(socket, request.Method);
        }

        private void OnRequest(HttpRequest request, HttpResponse response)
        {
            switch (request.Method)
            {
                case Net.HttpMethod.Get:
                    OnGet(request, response);
                    break;

                case Net.HttpMethod.Post:
                    OnPost(request, response);
                    break;

                case Net.HttpMethod.Nonsupport:
                    OnNonsupport(request, response);
                    break;
            }
        }

        private void AfterRequest(BaseSocket socket, Net.HttpMethod method)
        {
            switch (method)
            {
                case Net.HttpMethod.Get:
                    AfterGet(socket);
                    break;

                case Net.HttpMethod.Post:
                    AfterPost(socket);
                    break;

                case Net.HttpMethod.Nonsupport:
                    AfterNonsupport(socket);
                    break;
            }
        }

        /// <summary>
        /// 当请求方法为 GET 时执行，默认返回 405
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="response">响应</param>
        public virtual void OnGet(HttpRequest request, HttpResponse response)
        {
            response.StateCode = "405";
            response.Description = HttpState.Description["405"];
        }

        /// <summary>
        /// 当 GET 响应头发送之后执行
        /// </summary>
        /// <param name="socket">客户端 Socket</param>
        public virtual void AfterGet(BaseSocket socket)
        {
        }

        /// <summary>
        /// 当请求方法为 POST 时执行，默认返回 405
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="response">响应</param>
        public virtual void OnPost(HttpRequest request, HttpResponse response)
        {
            response.StateCode = "405";
            response.Description = HttpState.Description["405"];
        }

        /// <summary>
        /// 当 POST 响应头发送之后执行
        /// </summary>
        /// <param name="socket">客户端 Socket</param>
        public virtual void AfterPost(BaseSocket socket)
        {
        }

        /// <summary>
        /// 当请求方法为 Nonsupport 时执行，默认返回 501
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="response">响应</param>
        public virtual void OnNonsupport(HttpRequest request, HttpResponse response)
        {
            response.StateCode = "501";
            response.Description = HttpState.Description["501"];
        }

        /// <summary>
        /// 当 Nonsupport 响应头发送之后执行
        /// </summary>
        /// <param name="socket">客户端 Socket</param>
        public virtual void AfterNonsupport(BaseSocket socket)
        {
        }
    }
}