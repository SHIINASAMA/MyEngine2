namespace MyEngine2.Common.Net
{
    public class HttpResponse : HttpPackage
    {
        public string Version { get; set; } = "HTTP/1.1";

        public string StateCode { get; set; } = "200";

        public string Description { get; set; } = "";

        public HttpResponse(string version, string stateCode, string description, BaseSocket baseSocket) : base(baseSocket)
        {
            Version = version;
            StateCode = stateCode;
            Description = description;
        }

        public HttpResponse(string version, string stateCode, string description)
        {
            Version = version;
            StateCode = stateCode;
            Description = description;
        }

        public new void Clear()
        {
            base.Clear();
            Version = "HTTP/1.1";
            StateCode = "200";
            Description = "";
        }
    }
}