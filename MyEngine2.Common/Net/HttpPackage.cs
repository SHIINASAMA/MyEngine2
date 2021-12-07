namespace MyEngine2.Common.Net
{
    public class HttpPackage : Dictionary<string, string>
    {
        public string Host
        {
            get { return GetHeader("Host"); }
            set { SetHeader("Host", value); }
        }

        public string Location
        {
            get { return GetHeader("Location"); }
            set { SetHeader("Location", value); }
        }

        public string Connection
        {
            get { return GetHeader("Connection"); }
            set { SetHeader("Connection", value); }
        }

        public string Accept
        {
            get { return GetHeader("Accept"); }
            set { SetHeader("Accept", value); }
        }

        public string Server
        {
            get { return GetHeader("Server"); }
            set { SetHeader("Server", value); }
        }

        public string Pragma
        {
            get { return GetHeader("Pragma"); }
            set { SetHeader("Pragma", value); }
        }

        public string ContentType
        {
            get { return GetHeader("Content-Type"); }
            set { SetHeader("ContentType", value); }
        }

        public string ContentLength
        {
            get { return GetHeader("Content-Length"); }
            set { SetHeader("Content-Length", value); }
        }

        public string UserAgent
        {
            get { return GetHeader("UserAgent"); }
            set { SetHeader("UserAgent", value); }
        }

        public string Date
        {
            get { return GetHeader("Date"); }
            set { SetHeader("Date", value); }
        }

        public HttpPackage()
        {
        }

        public void SetHeader(string key, string value)
        {
            Add(key, value);
        }

        public string GetHeader(string key)
        {
            return base[key] ?? "";
        }
    }
}