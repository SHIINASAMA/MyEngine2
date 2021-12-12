namespace MyEngine2.Common.Net
{
    /// <summary>
    /// Http 解析器
    /// </summary>
    public class HttpParser
    {
        /// <summary>
        /// 从 Socket 中解析请求报头
        /// </summary>
        /// <param name="socket">套接字</param>
        /// <returns>Http 报头</returns>
        public static HttpRequest? ParseRequestFromSocket(BaseSocket socket)
        {
            HttpRequest? request = null;

            string firstLine = socket.ReadLine();
            string[] firstLineElements = firstLine.Split(' ');
            string headerLine;
            if (firstLineElements.Length == 3)
            {
                request = new HttpRequest(
                    firstLineElements[2],
                    firstLineElements[1],
                    StringToMethod(firstLineElements[0])
                    );

                while ((headerLine = socket.ReadLine()).Length != 0)
                {
                    string[] pair = headerLine.Split(": ");
                    request.SetHeader(pair[0], pair[1]);
                }
            }
            return request;
        }

        /// <summary>
        /// 从 Socket 中解析响应报头
        /// </summary>
        /// <param name="socket">套接字</param>
        /// <returns>Http 响应报头</returns>
        public static HttpResponse? ParseResponseFromSocket(BaseSocket socket)
        {
            HttpResponse? response = null;

            string firstLine = socket.ReadLine();
            string[] firstLineElements = firstLine.Split(" ");
            string headerLine;
            if (firstLineElements.Length == 2 || firstLineElements.Length == 3)
            {
                response = new HttpResponse(
                    firstLineElements[0],
                    firstLineElements[1],
                    firstLineElements.Length == 3 ? firstLineElements[2] : ""
                    );

                while ((headerLine = socket.ReadLine()).Length != 0)
                {
                    string[] pair = headerLine.Split(": ");
                    response.SetHeader(pair[0], pair[1]);
                }
            }
            return response;
        }

        /// <summary>
        /// 字符串转 Http 方法枚举
        /// </summary>
        /// <param name="method">方法字符串</param>
        /// <returns>Http 方法枚举</returns>
        public static HttpMethod StringToMethod(string method)
        {
            switch (method.ToUpper())
            {
                case "GET":
                    return HttpMethod.Get;

                case "POST":
                    return HttpMethod.Post;

                default:
                    return HttpMethod.Nonsupport;
            }
        }

        /// <summary>
        /// 方法转对应字符串
        /// </summary>
        /// <param name="method">Http 方法枚举</param>
        /// <returns>方法字符串</returns>
        public static string MethodToString(HttpMethod method)
        {
            return method.ToString();
        }
    }
}