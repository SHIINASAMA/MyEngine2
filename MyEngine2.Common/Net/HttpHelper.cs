namespace MyEngine2.Common.Net
{
    public class HttpHelper
    {
        public BaseSocket BaseSocket { get; }

        public HttpHelper(BaseSocket baseSocket)
        {
            BaseSocket = baseSocket;
        }

        public HttpRequest? ReadRequest()
        {
            return HttpParser.ParseRequestFromSocket(BaseSocket);
        }

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

        public HttpResponse? ReadResponse()
        {
            return HttpParser.ParseResponseFromSocket(BaseSocket);
        }

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