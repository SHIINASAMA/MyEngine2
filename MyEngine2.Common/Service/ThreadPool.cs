namespace MyEngine2.Common.Service
{
    public class ThreadPool
    {
        /// <summary>
        /// 线程池名称
        /// </summary>
        public string PoolName { get; }

        /// <summary>
        /// 线程数目
        /// </summary>
        public int ThreadCount { get; }

        /// <summary>
        /// 任务队列长度
        /// </summary>
        public int QueueLength { get; }

        // 任务队列
        private Queue<Action> Tasks;

        // 任务互斥锁
        private Mutex Mutex = new();

        // 线程组
        private LinkedList<Thread> Threads = new();

        private Semaphore Semaphore;

        /// <summary>
        /// 线程池是否退出
        /// </summary>
        public bool IsShutdown
        { get { return _Shutdown == 0 ? false : true; } }

        private volatile int _Shutdown = 0;

        /// <summary>
        /// 初始化一个线程池
        /// </summary>
        /// <param name="poolName">线程池名称</param>
        /// <param name="threadCount">线程数目</param>
        /// <param name="queueLength">任务队列长度</param>
        public ThreadPool(string poolName, int threadCount, int queueLength = 256)
        {
            PoolName = poolName;
            ThreadCount = threadCount;
            QueueLength = queueLength;

            Tasks = new(queueLength);
            Semaphore = new(0, queueLength);

            for (int i = 0; i < threadCount; i++)
            {
                Thread subthread = new(new ThreadStart(ThreadProc));
                subthread.Name = poolName + "_" + i;
                Threads.AddLast(subthread);
                subthread.Start();
            }
        }

        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="action">任务</param>
        public void Execute(Action action)
        {
            Semaphore.Release();
            Tasks.Enqueue(action);
        }

        /// <summary>
        /// 终止线程池，阻塞至所有子线程当前任务完成
        /// </summary>
        public void Shutdown()
        {
            Interlocked.Decrement(ref _Shutdown);
            foreach (var thread in Threads)
            {
                thread.Join();
            }
        }

        /// <summary>
        /// 线程过程
        /// </summary>
        private void ThreadProc()
        {
            while (_Shutdown == 0)
            {
                if (Semaphore.WaitOne(2000))
                {
                    Action? action;
                    if (Tasks.TryDequeue(out action))
                    {
                        if (action != null)
                        {
                            action();
                        }
                    }
                }
            }
        }
    }
}