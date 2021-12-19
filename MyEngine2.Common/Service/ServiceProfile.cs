using System.Xml.Serialization;

namespace MyEngine2.Common.Service
{
    [Serializable]
    [XmlRoot("ServiceProfile")]
    public class ServiceProfile
    {
        [XmlElement("Server")]
        public ServerProfile Server { get; set; } = new();

        [XmlElement("Logger")]
        public LoggerProfile Logger { get; set; } = new();

        [XmlElement("Net")]
        public NetProfile Net { get; set; } = new();

        [Serializable]
        public class ServerProfile
        {
            [XmlElement("Name")]
            public string Name { get; set; } = "MyEngine2";

            [XmlElement("Address")]
            public string Address { get; set; } = "127.0.0.1";

            [XmlElement("Port")]
            public int Port { get; set; } = 8080;

            [XmlElement("Backlog")]
            public int Backlog { get; set; } = 64;

            [XmlElement("ThreadPool")]
            public ThreadPoolProfile ThreadPool { get; set; } = new();

            [XmlElement("Root")]
            public string Root { get; set; } = "Web";

            [XmlElement("HomePage")]
            public PageProfile HomePage { get; set; } = new(false, "/index.html");

            [XmlElement("NotFoundPage")]
            public PageProfile NotFoundPage { get; set; } = new(false, "/not_found.html");

            [Serializable]
            public class ThreadPoolProfile
            {
                [XmlElement("Name")]
                public string Name { get; set; } = "ThreadPool";

                [XmlElement("ThreadCount")]
                public int ThreadCount { get; set; } = 16;

                [XmlElement("QueueLength")]
                public int QueueLength { get; set; } = 256;
            }

            [Serializable]
            public class PageProfile
            {
                [XmlElement("Enable")]
                public bool Enable { get; set; } = false;

                [XmlElement("Path")]
                public string Path { get; set; } = "page.html";

                public PageProfile()
                {
                }

                public PageProfile(bool enable, string path)
                {
                    Enable = enable;
                    Path = path;
                }
            }
        }

        [Serializable]
        public class LoggerProfile
        {
            [XmlElement("ConsoleAppender")]
            public ConsoleAppenderProfile ConsoleAppender { get; set; } = new();

            [XmlElement("FileAppender")]
            public FileAppenderProfile FileAppender { get; set; } = new();

            [Serializable]
            public class ConsoleAppenderProfile
            {
                [XmlElement("Enable")]
                public bool Enable { get; set; } = true;

                [XmlElement("Level")]
                public Logger.Level Level { get; set; } = Common.Logger.Level.Debug;

                [XmlElement("Pattern")]
                public string Pattern { get; set; } = "[%lv] %tm %m";

                [XmlElement("DatePattern")]
                public string DatePattern { get; set; } = "HH:mm:ss";
            }

            [Serializable]
            public class FileAppenderProfile
            {
                [XmlElement("Enable")]
                public bool Enable { get; set; } = false;

                [XmlElement("Level")]
                public Logger.Level Level { get; set; } = Common.Logger.Level.Debug;

                [XmlElement("NamePattern")]
                public string NamePattern { get; set; } = "'/log/'HH:mm:ss'.log'";

                [XmlElement("Pattern")]
                public string Pattern { get; set; } = "[%lv %tm %m]";

                [XmlElement("DatePattern")]
                public string DatePattern { get; set; } = "HH:mm:ss";
            }
        }

        [Serializable]
        public class NetProfile
        {
            [XmlElement("AcceptRanges")]
            public bool AcceptRanges { get; set; } = false;

            [XmlElement("EnableKeepAlive")]
            public bool EnableKeepAlive { get; set; } = false;

            [XmlElement("ReceiveTimeOut")]
            public int ReceiveTimeOut { get; set; } = 2000;

            [XmlElement("SendTimeOut")]
            public int SendTimeOut { get; set; } = 0;

            [XmlElement("MaxRequestTimes")]
            public int MaxRequestTimes { get; set; } = 64;

            [XmlElement("MaxHeaderLength")]
            public int MaxHeaderLength { get; set; } = 81920;

            [XmlElement("MaxHeadersLength")]
            public int MaxHeadersLength { get; set; } = 8192000;
        }
    }
}