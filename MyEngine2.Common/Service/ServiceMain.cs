using System.Net;
using MyEngine2.Common.Net;

namespace MyEngine2.Common.Service
{
    public class ServiceMain
    {
        private ServiceProfile ServiceProfile;
        private BaseSocket BaseSocket;
        private ThreadPool ThreadPool;
        private ServletSet ServletSet; // 用户自定义的 Servlet
        private BaseServlet FileServlet; // 系统用于传输文件的 Servlet
        private Thread AcceptThread;
        private ReaderWriterLock ServletLock = new();

        #region LoggerWrapper

        private void Debug(string message) => LoggerManager.Logger.Debug(message);

        private void Info(string message) => LoggerManager.Logger.Info(message);

        private void Warn(string message) => LoggerManager.Logger.Warn(message);

        private void Error(string message) => LoggerManager.Logger.Error(message);

        private void Fatal(string message) => LoggerManager.Logger.Fatal(message);

        #endregion LoggerWrapper

        public ServiceMain(ServiceProfile serviceProfile)
        {
            ServiceProfile = serviceProfile;

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
            }
            else
            {
                FileServlet = new FileServlet(ServiceProfile.Server, ServiceProfile.Net.EnableKeepAlive);
            }
            Info("[OK] ServletSet");

            // 5.初始化循环监听线程
            AcceptThread = new(new ThreadStart(MainLoop));
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
            AcceptThread.Interrupt();
            ThreadPool.Shutdown();
        }

        private void MainLoop()
        {
            while (true)
            {
                try
                {
                }
                catch (ThreadInterruptedException e)
                {
                    break;
                }
            }
        }

        private void SubThreadProc()
        {
        }
    }
}