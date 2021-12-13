using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyEngine2.Common.Service;
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
        public void TestMainService()
        {
            ServiceMain main = new(new ServiceProfile());
        }
    }
}