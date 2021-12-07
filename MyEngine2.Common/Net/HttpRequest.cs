﻿namespace MyEngine2.Common.Net
{
    public class HttpRequest : HttpPackage
    {
        public string Version { get; set; } = "HTTP/1.1";

        public string RawUrl { get; set; } = "/";

        public HttpMethod Method { get; set; } = HttpMethod.Nonsupport;

        public string Url
        { get { return QueryString.Url; } }

        public QueryString QueryString;

        public HttpRequest(string version, string rawUrl, HttpMethod method)
        {
            Version = version;
            RawUrl = rawUrl;
            Method = method;

            QueryString = new(rawUrl);
        }

        public new void Clear()
        {
            base.Clear();
            Version = "HTTP/1.1";
            RawUrl = "/";
            Method = HttpMethod.Nonsupport;
        }
    }
}