using System;
using System.Collections.Generic;


namespace ThreadSupport
{
    public sealed class QueueManager
    {
        static readonly QueueManager _instance = new QueueManager();
        private readonly Dictionary<String, BlockingQueue<ThreadMessage>> _qList = new Dictionary<String, BlockingQueue<ThreadMessage>>();
        private static readonly object syncRoot = new Object();

        public static QueueManager Instance
        {
            get
            {
                lock (syncRoot)  // Make sure we are thread safe
                {
                    return _instance;
                }
            }
        }

        public void AddQueue(ref BlockingQueue<ThreadMessage> q, String key)
        {
            _qList.Add(key, q);
        }

        public BlockingQueue<ThreadMessage> GetQueue(String key)
        {
            BlockingQueue<ThreadMessage> result = null;
            if (_qList.ContainsKey(key))
            {
                result = _qList[key];
            }
            return result;

        }
    }
}
