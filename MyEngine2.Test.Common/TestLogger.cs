using System;
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
            Event @event = new Event(Level.DEBUG, "This is a test");
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
            Event @event1 = new Event(Level.INFO, "This is a test");
            Event @event2 = new Event(Level.WARN, "This is a test");
            Event @event3 = new Event(Level.FATAL, "This is a test");

            Formatter formatter = new Formatter("[%lv] %tm %m %%");
            Console.WriteLine(formatter.Format(@event1));
            Console.WriteLine(formatter.Format(@event2));
            Console.WriteLine(formatter.Format(@event3));
        }
    }
}