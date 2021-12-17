namespace MyEngine2.Common.Service
{
    /// <summary>
    /// 线程安全的 Servlet 集合
    /// </summary>
    public class ServletSet : Dictionary<string, BaseServlet>
    {
        private ReaderWriterLockSlim ReaderWriterLockSlim = new();

        /// <summary>
        /// 注册 Servlet - 线程安全
        /// </summary>
        /// <param name="url">路径</param>
        /// <param name="baseServlet">Servlet</param>
        public void Register(string url, BaseServlet baseServlet)
        {
            ReaderWriterLockSlim.EnterWriteLock();
            try
            {
                base[url] = baseServlet;
            }
            finally
            {
                ReaderWriterLockSlim.ExitWriteLock();
            }
        }

        /// <summary>
        /// 批量注册 Servlet - 线程安全
        /// </summary>
        /// <param name="keyValuePairs">键值对</param>
        public void Register(List<KeyValuePair<string, BaseServlet>> keyValuePairs)
        {
            ReaderWriterLockSlim.EnterWriteLock();
            try
            {
                foreach (var pair in keyValuePairs)
                {
                    base[pair.Key] = pair.Value;
                }
            }
            finally
            {
                ReaderWriterLockSlim.ExitWriteLock();
            }
        }

        /// <summary>
        /// 取消注册 Servlet - 线程安全
        /// </summary>
        /// <param name="url">路径</param>
        public void UnRegister(string url)
        {
            ReaderWriterLockSlim.EnterWriteLock();
            try
            {
                base.Remove(url);
            }
            finally
            {
                ReaderWriterLockSlim.ExitWriteLock();
            }
        }

        /// <summary>
        /// 清空 Servlet 集合 - 线程安全
        /// </summary>
        public void UnRegisterAll()
        {
            ReaderWriterLockSlim.EnterWriteLock();
            try
            {
                base.Clear();
            }
            finally
            {
                ReaderWriterLockSlim.ExitWriteLock();
            }
        }

        /// <summary>
        /// 查找 Servlet - 线程安全
        /// </summary>
        /// <param name="url">路径</param>
        /// <returns>Servlet</returns>
        public BaseServlet? FindServlet(string url)
        {
            ReaderWriterLockSlim.EnterReadLock();
            try
            {
                if (base.ContainsKey(url))
                {
                    return base[url];
                }
                else
                {
                    return null;
                }
            }
            finally
            {
                ReaderWriterLockSlim.ExitReadLock();
            }
        }
    }
}