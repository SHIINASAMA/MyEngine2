namespace MyEngine2.Common.Net
{
    /// <summary>
    /// Http 帮助类
    /// </summary>
    public class HttpHelper
    {
        /// <summary>
        /// 用于通信的套接字
        /// </summary>
        public BaseSocket BaseSocket { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="baseSocket">套接字</param>
        public HttpHelper(BaseSocket baseSocket)
        {
            BaseSocket = baseSocket;
        }

        /// <summary>
        /// 读取报文头部
        /// </summary>
        /// <returns>Http 请求头</returns>
        public HttpRequest? ReadRequest()
        {
            return HttpParser.ParseRequestFromSocket(BaseSocket);
        }

        /// <summary>
        /// 写入报文头部
        /// </summary>
        /// <param name="httpRequest">Http 请求头</param>
        public void WriteRequest(HttpRequest httpRequest)
        {
            BaseSocket.WriteLine(
                string.Format("{0} {1} {2}",
                HttpParser.MethodToString(httpRequest.Method),
                httpRequest.RawUrl,
                httpRequest.Version)
                );
            foreach (var pair in httpRequest)
            {
                BaseSocket.WriteLine(string.Format("{0}: {1}", pair.Key, pair.Value));
            }
            BaseSocket.WriteLine("");
        }

        /// <summary>
        /// 读取 Http 响应报头
        /// </summary>
        /// <returns>Http 响应报头</returns>
        public HttpResponse? ReadResponse()
        {
            return HttpParser.ParseResponseFromSocket(BaseSocket);
        }

        /// <summary>
        /// 写入 Http 响应报头
        /// </summary>
        /// <param name="httpResponse">Http 响应报头</param>
        public void WriteResponse(HttpResponse httpResponse)
        {
            BaseSocket.WriteLine(
                String.Format("{0} {1} {2}",
                httpResponse.Version,
                httpResponse.StateCode,
                httpResponse.Description)
                );

            foreach (var pair in httpResponse)
            {
                BaseSocket.WriteLine(string.Format("{0}: {1}", pair.Key, pair.Value));
            }
            BaseSocket.WriteLine("");
        }
    }
}