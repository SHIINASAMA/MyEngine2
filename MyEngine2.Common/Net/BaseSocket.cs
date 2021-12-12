using System.Text;
using System.Net;
using System.Net.Sockets;

namespace MyEngine2.Common.Net
{
    /// <summary>
    /// 用于 Http 传输的基础套接字
    /// </summary>
    public class BaseSocket : Socket
    {
        /// <summary>
        /// 头部单行最大长度
        /// </summary>
        public int MaxHeaderLength { get; set; } = 1024 * 80;

        /// <summary>
        /// 头部最大长度
        /// </summary>
        public int MaxHeadersLength { get; set; } = 1024 * 8000;

        /// <summary>
        /// 头部长度
        /// </summary>
        public int HeadersLength
        { get { return _HeadersLength; } }

        private int _HeadersLength = 0;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="addressFamily">地址族</param>
        public BaseSocket(AddressFamily addressFamily) : base(addressFamily, SocketType.Stream, ProtocolType.Tcp)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="safeSocketHandle">安全套接字句柄</param>
        public BaseSocket(SafeSocketHandle safeSocketHandle) : base(safeSocketHandle)
        {
        }

        /// <summary>
        /// 绑定
        /// </summary>
        /// <param name="iPAddress">IP 地址</param>
        /// <param name="port">端口</param>
        public void Bind(IPAddress iPAddress, int port)
        {
            Bind(new IPEndPoint(iPAddress, port));
        }

        /// <summary>
        /// 接收连接
        /// </summary>
        /// <returns>客户端套接字</returns>
        public new BaseSocket Accept()
        {
            return new BaseSocket(base.Accept().SafeHandle);
        }

        /// <summary>
        /// 读取一行头部 - 自动丢弃 "\r\n"
        /// </summary>
        /// <returns>一行头部字符串</returns>
        /// <exception cref="HttpException"></exception>
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
                    if (0 == Receive(buffer, 1, SocketFlags.None))
                    {
                        throw new HttpException("BaseSocket 没有读取到任何字节");
                    }
                    ch = Encoding.ASCII.GetString(buffer)[0];
                    count++;
                    if (count >= MaxHeaderLength)
                    {
                        throw new HttpException("Http 头部单行过长");
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
                    throw new HttpException("Header 头部过长");
                }
            }
        }

        /// <summary>
        /// 写入一行头部字符串
        /// </summary>
        /// <param name="line">一行头部字符串</param>
        /// <returns>发送字节数</returns>
        /// <exception cref="HttpException"></exception>
        public int WriteLine(string line)
        {
            _HeadersLength += line.Length;
            if (_HeadersLength > MaxHeadersLength)
            {
                throw new HttpException("Header 头部过长");
            }
            else if (line.Length > MaxHeaderLength + 2)
            {
                throw new HttpException("Http 头部单行过长");
            }
            else
            {
                return base.Send(Encoding.ASCII.GetBytes(line + "\r\n"));
            }
        }
    }
}