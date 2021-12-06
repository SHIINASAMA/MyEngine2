using System.Text;
using System.Net;
using System.Net.Sockets;

namespace MyEngine2.Common.Net
{
    public class BaseSocket : Socket
    {
        public int MaxHeaderLength { get; } = 1024 * 80;
        public int MaxHeadersLength { get; } = 1024 * 8000;

        public int HeadersLength
        { get { return _HeadersLength; } }

        private int _HeadersLength = 0;

        public BaseSocket(AddressFamily addressFamily, int maxHeaderLength = 1024 * 80, int maxHeadersLength = 1024 * 8000) : base(addressFamily, SocketType.Stream, ProtocolType.Tcp)
        {
            MaxHeaderLength = maxHeaderLength;
            MaxHeadersLength = maxHeadersLength;
        }

        public BaseSocket(SafeSocketHandle safeSocketHandle) : base(safeSocketHandle)
        {
        }

        public void Bind(IPAddress iPAddress, int port)
        {
            Bind(new IPEndPoint(iPAddress, port));
        }

        public new BaseSocket Accept()
        {
            return new BaseSocket(base.Accept().SafeHandle);
        }

        public string ReadLine()
        {
            byte[] buffer = new byte[1];
            char ch;
            bool hit = false;// 上一个字符是否为 '\r'
            int count = 0;
            StringBuilder stringBuilder = new StringBuilder();
            try
            {
                while (true)
                {
                    Receive(buffer, 1, SocketFlags.None);
                    ch = Encoding.ASCII.GetString(buffer)[0];
                    count++;
                    if (count >= MaxHeaderLength)
                    {
                        throw new HttpHeaderException("Http 头部单行过长");
                    }
                    else if (ch == '\r')
                    {
                        hit = true;
                        continue;
                    }
                    else if (ch == '\n')
                    {
                        if (hit)
                        {
                            return stringBuilder.ToString();
                        }
                        else
                        {
                            stringBuilder.Append(ch);
                            hit = false;
                        }
                    }
                    else
                    {
                        stringBuilder.Append(ch);
                        continue;
                    }
                }
            }
            finally
            {
                _HeadersLength += count;
                if (_HeadersLength > MaxHeadersLength)
                {
                    throw new BaseSocketException("Header 头部过长");
                }
            }
        }

        public int WriteLine(string line)
        {
            _HeadersLength += line.Length;
            if (_HeadersLength > MaxHeadersLength)
            {
                throw new BaseSocketException("Header 头部过长");
            }
            else if (line.Length > MaxHeaderLength + 2)
            {
                throw new HttpHeaderException("Http 头部单行过长");
            }
            else
            {
                return base.Send(Encoding.ASCII.GetBytes(line + "\r\n"));
            }
        }
    }
}