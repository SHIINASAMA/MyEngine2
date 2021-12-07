namespace MyEngine2.Common.Net
{
    public class HttpParser
    {
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

        public static string MethodToString(HttpMethod method)
        {
            return method.ToString();
        }
    }
}