using MyEngine2.Common.Net;

namespace MyEngine2.Common.Service
{
    /// <summary>
    /// 套接字管理器
    /// </summary>
    public class SocketManager
    {
        /// <summary>
        /// 网络配置文件
        /// </summary>
        public ServiceProfile.NetProfile NetProfile { get; }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="netProfile">网络配置文件</param>
        public SocketManager(ServiceProfile.NetProfile netProfile)
        {
            NetProfile = netProfile;
        }

        /// <summary>
        /// 初始化套接字
        /// </summary>
        /// <param name="socket">套接字</param>
        public void InitSocket(BaseSocket socket)
        {
            socket.MaxHeaderLength = NetProfile.MaxHeaderLength;
            socket.MaxHeadersLength = NetProfile.MaxHeadersLength;
            socket.ReceiveTimeout = NetProfile.ReceiveTimeOut;
            socket.SendTimeout = NetProfile.SendTimeOut;
        }
    }
}