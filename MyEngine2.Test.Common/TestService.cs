using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyEngine2.Common.Net;
using MyEngine2.Common.Service;
using System.IO;
using System.Threading;
using System.Xml.Serialization;

namespace MyEngine2.Test.Common
{
    [TestClass]
    public class TestService
    {
        [TestMethod]
        public void TestThreadPool()
        {
            MyEngine2.Common.Service.ThreadPool threadPool = new("ThreadPool", 8, 128);
            for (int i = 0; i < 200; i++)
            {
                try
                {
                    threadPool.Execute(ThreadProc, i);
                }
                catch (SemaphoreFullException ex)
                {
                    LoggerManager.Logger.Warn(ex.Message);
                }
            }
            Thread.Sleep(1000 * 10);
            threadPool.Shutdown();
        }

        public void ThreadProc(object? i)
        {
            LoggerManager.Logger.Info(i.ToString());
        }

        [TestMethod]
        public void TestFileServlet()
        {
            BaseSocket serverSocket = new BaseSocket(System.Net.Sockets.AddressFamily.InterNetwork);
            serverSocket.Bind(System.Net.IPAddress.Loopback, 8080);
            serverSocket.Listen(10);

            BaseSocket clientSocket = serverSocket.Accept();
            FileServlet fileServlet = new FileServlet(new ServiceProfile.ServerProfile(), false);
            fileServlet.Exec(clientSocket);
            clientSocket.Shutdown(System.Net.Sockets.SocketShutdown.Both);
            clientSocket.Close();
        }

        [TestMethod]
        public void TestRangeFileServlet()
        {
            BaseSocket serverSocket = new BaseSocket(System.Net.Sockets.AddressFamily.InterNetwork);
            serverSocket.Bind(System.Net.IPAddress.Loopback, 8080);
            serverSocket.Listen(10);

            BaseSocket clientSocket = serverSocket.Accept();
            FileServlet fileServlet = new RangeFileServlet(new ServiceProfile.ServerProfile(), false);
            fileServlet.Exec(clientSocket);
            clientSocket.Shutdown(System.Net.Sockets.SocketShutdown.Both);
            clientSocket.Close();
        }

        [TestMethod]
        public void TestXmlSerializer()
        {
            ServiceProfile profile = new();
            XmlSerializer serializer = new(typeof(ServiceProfile));
            serializer.Serialize(new FileStream("MyConfig.xml", FileMode.OpenOrCreate), profile);
        }
    }
}