using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyEngine2.Common.Logger;
using MyEngine2.Common.Net;
using System;

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
        public void TestParser()
        {
            BaseSocket serverSocket = new BaseSocket(System.Net.Sockets.AddressFamily.InterNetwork);
            serverSocket.Bind(System.Net.IPAddress.Loopback, 8080);
            serverSocket.Listen(10);

            BaseSocket clientSocket = serverSocket.Accept();
            HttpRequest? request = HttpParser.ParseRequestFromSocket(clientSocket);
            if (request == null) Assert.IsTrue(false);

            LoggerManager.Logger.Info(string.Format("{0} {1} {2}", request.Method, request.RawUrl, request.Version));
            foreach (var pair in request)
            {
                LoggerManager.Logger.Info(pair.Key + ": " + pair.Value);
            }

            clientSocket.Shutdown(System.Net.Sockets.SocketShutdown.Both);
            clientSocket.Close();
            clientSocket.Dispose();
            serverSocket.Close();
            serverSocket.Dispose();
        }

        [TestMethod]
        public void TestEncoder()
        {
            string message = @"これは!Encode消 息Test$.";
            string encodedMessage = PercentEncoder.Encode(message);
            LoggerManager.Logger.Info(string.Format("encodedMessage: {0}", encodedMessage));
            string decodedMessage = PercentDecoder.Decode(encodedMessage);
            LoggerManager.Logger.Info(string.Format("decodedMessage: {0}", decodedMessage));
            Assert.IsTrue(message.Equals(decodedMessage));
        }

        [TestMethod]
        public void TestQueryString()
        {
            // UTF-8 URl -> /query?name=かおる&pwd=&uuid=1000000
            string rawUrl = @"/query?name=%E3%81%8B%E3%81%8A%E3%82%8B&pwd=&uuid=1000000";
            QueryString queryString = new QueryString(rawUrl);
            LoggerManager.Logger.Info(queryString.Url);
            foreach (var pair in queryString)
            {
                LoggerManager.Logger.Info(String.Format("{0} = {1}", pair.Key, pair.Value));
            }
        }
    }
}