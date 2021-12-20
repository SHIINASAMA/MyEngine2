using MyEngine2.Common.Net;

namespace MyEngine2.Common.Service
{
    /// <summary>
    /// Socket 上下文
    /// </summary>
    public class SocketContext : IDisposable
    {
        /// <summary>
        /// 客户端套接字
        /// </summary>
        public BaseSocket Socket { get; }

        /// <summary>
        /// KeepAlive
        /// </summary>
        public bool KeepAlive { get; set; } = false;

        /// <summary>
        /// 请求次数
        /// </summary>
        public int Times
        {
            get { return _Times; }
        }

        private int _Times = 0;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="socket">客户端套接字</param>
        public SocketContext(BaseSocket socket)
        {
            Socket = socket;
        }

        /// <summary>
        /// 当请求时手动调用， Time + 1
        /// </summary>
        public void Requested()
        {
            _Times++;
        }

        public void Dispose()
        {
            if (Socket.Connected)
            {
                Socket.Shutdown(System.Net.Sockets.SocketShutdown.Both);
                Socket.Close();
            }
        }
    }
}