using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyEngine2.Common.Logger;
using MyEngine2.Common.Net;

namespace MyEngine2.Test.Common
{
    [TestClass]
    public class TestNet
    {
        [TestMethod]
        public void TestBaseSocket()
        {
            BaseSocket serverSocket = new BaseSocket(System.Net.Sockets.AddressFamily.InterNetwork);
            serverSocket.Bind(System.Net.IPAddress.Loopback, 8080);
            serverSocket.Listen(10);

            BaseSocket clientSocket = serverSocket.Accept();
            while (true)
            {
                string line = clientSocket.ReadLine();
                if (line.Length != 0)
                {
                    // System.Console.WriteLine(line);
                    LoggerManager.Logger.Info(line);
                }
                else
                {
                    break;
                }
            }

            clientSocket.WriteLine("HTTP/1.1 302");
            clientSocket.WriteLine("Location: https://www.baidu.com");
            clientSocket.WriteLine("");
            clientSocket.Shutdown(System.Net.Sockets.SocketShutdown.Both);
            clientSocket.Close();
            clientSocket.Dispose();
            serverSocket.Close();
            serverSocket.Dispose();
        }

        [TestMethod]
        public void TestHttpPackage()
        {
            HttpPackage headers = new();
            headers.Location = "localhost";
            headers.Server = "MyEngine2";

            BaseSocket serverSocket = new BaseSocket(System.Net.Sockets.AddressFamily.InterNetwork);
            serverSocket.Bind(System.Net.IPAddress.Loopback, 8080);
            serverSocket.Listen(10);

            BaseSocket clientSocket = serverSocket.Accept();
            while (clientSocket.ReadLine().Length != 0) ;

            clientSocket.WriteLine("HTTP/1.1 200 OK");
            foreach (var pair in headers)
            {
                clientSocket.WriteLine(pair.Key + ": " + pair.Value);
            }
            clientSocket.WriteLine("");

            clientSocket.Shutdown(System.Net.Sockets.SocketShutdown.Both);
            clientSocket.Close();
            clientSocket.Dispose();
            serverSocket.Close();
            serverSocket.Dispose();
        }

        [TestMethod]
        public void TestHttpRequestAndResponse()
        {
        }
    }
}