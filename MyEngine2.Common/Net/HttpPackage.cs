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

        private BaseSocket? BaseSocket;

        public bool Connect
        {
            get { return BaseSocket != null && BaseSocket.Connected; }
        }

        public HttpPackage(BaseSocket baseSocket)
        {
            BaseSocket = baseSocket;
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

        public int ReadBody(byte[] bytes, int offset, int size)
        {
            if (BaseSocket == null) throw new BaseSocketException("BaseSocket 尚未初始化");
            if (!BaseSocket.Connected) throw new BaseSocketException("BaseSocket 尚未连接");
            return BaseSocket.Receive(bytes, offset, size, System.Net.Sockets.SocketFlags.None);
        }

        public int WriteBody(byte[] bytes, int offset, int size)
        {
            if (BaseSocket == null) throw new BaseSocketException("BaseSocket 尚未初始化");
            if (!BaseSocket.Connected) throw new BaseSocketException("BaseSocket 尚未连接");
            return BaseSocket.Send(bytes, offset, size, System.Net.Sockets.SocketFlags.None);
        }
    }
}