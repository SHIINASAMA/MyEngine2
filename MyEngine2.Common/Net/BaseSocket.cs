using System.Text;
using System.Net;
using System.Net.Sockets;

namespace MyEngine2.Common.Net
{
    public class BaseSocket : Socket
    {
        public int MaxHeaderLength { get; } = 1024 * 80;

        public BaseSocket(AddressFamily addressFamily, int maxHeaderLength = 1024 * 80) : base(addressFamily, SocketType.Stream, ProtocolType.Tcp)
        {
            MaxHeaderLength = maxHeaderLength;
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
            StringBuilder stringBuilder = new StringBuilder();

            int count = 0;
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

        public int WriteLine(string line)
        {
            if (line.Length > MaxHeaderLength + 2)
            {
                throw new HttpHeaderException("Http 头部单行过长");
            }
            return base.Send(Encoding.ASCII.GetBytes(line + "\r\n"));
        }
    }
}