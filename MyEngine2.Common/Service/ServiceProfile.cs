namespace MyEngine2.Common.Service
{
    public class ServiceProfile
    {
        public ServerProfile Server { get; set; } = new();
        public LoggerProfile Logger { get; set; } = new();
        public NetProfile Net { get; set; } = new();

        public class ServerProfile
        {
            public string Name { get; set; } = "MyEngine2";
            public string Address { get; set; } = "localhost";
            public int Port { get; set; } = 8080;
            public int Backlog { get; set; } = 64;
            public ThreadPoolProfile ThreadPool { get; set; } = new();
            public string Root { get; set; } = "Web";
            public HomePageProfile HomePage { get; set; } = new();
            public NotFoundPageProfile NotFoundPage { get; set; } = new();

            public class ThreadPoolProfile
            {
                public string Name { get; set; } = "ThreadPool";
                public int ThreadCount { get; set; } = 16;
                public int QueueLength { get; set; } = 256;
            }

            public class HomePageProfile
            {
                public bool Enable { get; set; } = false;
                public string Path { get; set; } = "/index.html";
            }

            public class NotFoundPageProfile
            {
                public bool Enable { get; set; } = false;
                public string Path { get; set; } = "/not_found.html";
            }
        }

        public class LoggerProfile
        {
            public ConsoleAppenderProfile ConsoleAppender { get; set; } = new();
            public FileAppenderProfile FileAppender { get; set; } = new();

            public class ConsoleAppenderProfile
            {
                public bool Enable { get; set; } = true;
                public Logger.Level Level { get; set; } = Common.Logger.Level.Debug;
                public string Pattern { get; set; } = "[%lv %tm %m]";
                public string DatePattern { get; set; } = "HH:mm:ss";
            }

            public class FileAppenderProfile
            {
                public bool Enable { get; set; } = false;
                public Logger.Level Level { get; set; } = Common.Logger.Level.Debug;
                public string NamePattern { get; set; } = "'/log/'HH:mm:ss'.log'";
                public string Pattern { get; set; } = "[%lv %tm %m]";
                public string DatePattern { get; set; } = "HH:mm:ss";
            }
        }

        public class NetProfile
        {
            public bool AcceptRanges { get; set; } = false;
            public bool EnableKeepAlive { get; set; } = false;
            public int ReceiveTimeOut { get; set; } = 2000;
            public int SendTimeOut { get; set; } = 0;
            public int MaxRequestTimes { get; set; } = 64;
            public int MaxHeaderLength { get; set; } = 81920;
            public int MaxHeadersLength { get; set; } = 8192000;
        }
    }
}