using System.Net;
using MyEngine2.Common.Net;

namespace MyEngine2.Common.Service
{
    public class ServiceMain
    {
        private ServiceProfile ServiceProfile;
        private BaseSocket BaseSocket;
        private ThreadPool ThreadPool;
        private ServletSet ServletSet; // 用户自定义的 Servlet 集合
        private BaseServlet FileServlet; // 系统用于传输文件的 Servlet
        private Thread AcceptThread;

        #region LoggerWrapper

        private static void Debug(string message) => LoggerManager.Logger.Debug(message);

        private static void Info(string message) => LoggerManager.Logger.Info(message);

        private static void Warn(string message) => LoggerManager.Logger.Warn(message);

        private static void Error(string message) => LoggerManager.Logger.Error(message);

        private static void Fatal(string message) => LoggerManager.Logger.Fatal(message);

        #endregion LoggerWrapper

        public ServiceMain(ServiceProfile serviceProfile)
        {
            ServiceProfile = serviceProfile;
            Thread.CurrentThread.Name = "ServiceMain";

            // 1.初始化日志管理器
            LoggerManager.InitLogger(ServiceProfile.Logger);
            Info("[OK] Logger");

            // 2.初始化服务器 Socket
            try
            {
                BaseSocket = new(System.Net.Sockets.AddressFamily.InterNetwork);
                BaseSocket.Bind(IPAddress.Parse(ServiceProfile.Server.Address), ServiceProfile.Server.Port);
                Info(string.Format("[OK] Socket.Bind {0}:{1}", ServiceProfile.Server.Address, ServiceProfile.Server.Port));
                BaseSocket.Listen(ServiceProfile.Server.Backlog);
                Info(string.Format("[OK] Socket.Listen {0}", ServiceProfile.Server.Backlog));
                Info("[OK] Socket");
            }
            catch (Exception e)
            {
                Fatal(string.Format("[NO] Socket {0}", e.Message));
                Environment.Exit(-1);
                return;
            }

            // 3.初始化线程池
            ThreadPool = new(
                ServiceProfile.Server.ThreadPool.Name,
                ServiceProfile.Server.ThreadPool.ThreadCount,
                ServiceProfile.Server.ThreadPool.QueueLength
                );
            Info(string.Format("[OK] ThreadPool Name:{0} Threads:{1} QueueLength:{2}",
                ServiceProfile.Server.ThreadPool.Name,
                ServiceProfile.Server.ThreadPool.ThreadCount,
                ServiceProfile.Server.ThreadPool.QueueLength)
                );
            Info("[OK] ThreadPool");

            // 4.初始化服务集
            ServletSet = new();
            // 检测是否手动设置主页（HomePage）和未找到请求资源页面（NotFoundPage）
            if (ServiceProfile.Server.HomePage.Enable)
            {
                Info(string.Format("HomePage    : {0}", ServiceProfile.Server.HomePage.Path));
            }
            if (ServiceProfile.Server.NotFoundPage.Enable)
            {
                Info(string.Format("NoFoundPage : {0}", ServiceProfile.Server.NotFoundPage.Path));
            }
            // 检测是否开启断点续传，设置对应 Servlet
            if (ServiceProfile.Net.AcceptRanges)
            {
                FileServlet = new RangeFileServlet(ServiceProfile.Server, ServiceProfile.Net.EnableKeepAlive);
            }
            else
            {
                FileServlet = new FileServlet(ServiceProfile.Server, ServiceProfile.Net.EnableKeepAlive);
            }
            Info("[OK] ServletSet");

            // 5.初始化循环监听线程
            AcceptThread = new(new ThreadStart(MainLoop));
            AcceptThread.IsBackground = true;
            AcceptThread.Name = "AcceptThread";
            Info("[OK] AcceptThread");
        }

        /// <summary>
        /// 启动监听线程 - 非阻塞
        /// </summary>
        public void StartAccpet()
        {
            AcceptThread.Start();
            Info("[OK] AcceptThread Started");
        }

        /// <summary>
        /// 终止监听线程 - 非阻塞
        /// </summary>
        public void StopAccept()
        {
            BaseSocket.Close();
            ThreadPool.Shutdown();
        }

        /// <summary>
        /// 工作者主循环
        /// </summary>
        private void MainLoop()
        {
            while (true)
            {
                try
                {
                    BaseSocket clientSocket = BaseSocket.Accept();
                    ThreadPool.Execute(SubThreadProc, clientSocket);
                }
                catch (System.Net.Sockets.SocketException e)
                {
                    Warn(e.Message);
                    Info("Listening To Terminate");
                    break;
                }
            }
            Thread.Sleep(0);
        }

        /// <summary>
        /// 线程池子线程任务
        /// </summary>
        /// <param name="argv">参数，此处为客户端套接字</param>
        private void SubThreadProc(object? argv)
        {
            try
            {
                if (argv != null)
                {
                    BaseSocket socket = (BaseSocket)argv;
                    socket.ReceiveTimeout = ServiceProfile.Net.ReceiveTimeOut;
                    socket.SendTimeout = ServiceProfile.Net.SendTimeOut;
                    socket.MaxHeaderLength = ServiceProfile.Net.MaxHeaderLength;
                    socket.MaxHeadersLength = ServiceProfile.Net.MaxHeadersLength;

                    //todo SocketContext 包装 BaseSocket
                    using SocketContext context = new(socket);
                    do
                    {
                        FileServlet.Exec(context);
                    } while (context.Times <= ServiceProfile.Net.MaxRequestTimes && context.KeepAlive);
                }
            }
            catch (Exception e)
            {
                Warn(e.Message);
            }
        }
    }
}