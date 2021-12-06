using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyEngine2.Common.Logger;

namespace MyEngine2.Test.Common
{
    [TestClass]
    public class TestLogger
    {
        [TestMethod]
        public void TestEvent()
        {
            Event @event = new Event(Level.Debug, "This is a test");
            Console.WriteLine("Level: {0}", @event.Level);
            Console.WriteLine("Time: {0}", @event.Time);
            Console.WriteLine("Full File Name: {0}", @event.FullFileName);
            Console.WriteLine("File Name: {0}", @event.FileName);
            Console.WriteLine("Line Number: {0}", @event.LineNumber);
            Console.WriteLine("Thread ID: {0}", @event.ThreadId);
            Console.WriteLine("Thread Name: {0}", @event.ThreadName);
            Console.WriteLine("Message: {0}", @event.Message);
        }

        [TestMethod]
        public void TestFormatter()
        {
            Event @event1 = new Event(Level.Info, "This is a test");
            Event @event2 = new Event(Level.Warn, "This is a test");
            Event @event3 = new Event(Level.Fatal, "This is a test");

            Formatter formatter = new Formatter("[%lv] %tm %m %%");
            Console.WriteLine(formatter.Format(@event1));
            Console.WriteLine(formatter.Format(@event2));
            Console.WriteLine(formatter.Format(@event3));
        }

        [TestMethod]
        public void TestAppender()
        {
            Event @event = new Event(Level.Error, "This is a test");
            ConsoleAppender consoleAppender = new ConsoleAppender(new Formatter());
            consoleAppender.Append(@event);
        }

        private Logger logger = new Logger();

        [TestMethod]
        public void TestMyLogger()
        {
            Thread.CurrentThread.Name = "MainThread";
            Formatter formatter = new("[%lv] %tm %tn %ti %fn:%ln - %m", "yyyy-MM-dd HH:mm:ss");
            ConsoleAppender consoleAppender = new ConsoleAppender(formatter);

            logger.AddAppender(consoleAppender);
            logger.Debug("MainThread Message");
            logger.Info("MainThread Message");
            logger.Warn("MainThread Message");
            logger.Error("MainThread Message");
            logger.Fatal("MainThread Message");

            Thread subThread = new Thread(new ThreadStart(ThreadProc));
            subThread.Start();
            subThread.Join();
            logger.Info("MainThread Message");
        }

        public void ThreadProc()
        {
            Thread.CurrentThread.Name = "SubThread";
            logger.Info("SubThread Message");
        }
    }
}