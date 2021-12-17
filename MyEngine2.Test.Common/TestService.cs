using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyEngine2.Common.Net;
using MyEngine2.Common.Service;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

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
                    threadPool.Execute(ThreadProc);
                }
                catch (SemaphoreFullException ex)
                {
                    LoggerManager.Logger.Warn(ex.Message);
                }
            }
            Thread.Sleep(1000 * 10);
            threadPool.Shutdown();
        }

        public void ThreadProc()
        {
            LoggerManager.Logger.Info(Thread.CurrentThread.Name ?? "Unknown Thread");
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
    }
}