using System;
using System.Threading;
using Metrics;
using NLog;

namespace ThreadSupport
{

    /// <summary>
    /// This BaseThread class creates a BlockingQueue of type ThreadMessage for the given thread.  The name of the queue is 
    /// specified on the constructor.
    /// 
    /// Additionally:
    ///     this class creates and instantiates a thread, using the derived classes Runner() method.
    ///     this class adds the name of the queue to the queuemanager singleton object
    /// 
    /// </summary>
    public abstract class BaseThread 
    {
        private volatile bool _running;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        
        public string Qname { get; }

        public bool Running
        {
            get { return _running; }
            set { _running = value; }
        }

        private readonly BlockingQueue<ThreadMessage> _myQueue;
        public BlockingQueue<ThreadMessage> MyQueue
        {
            get { return _myQueue; }
        }
        
        public string ThreadName { get; }

        private Thread _thisThread;
        public Thread ThisThread
        {
            get { return _thisThread; }
        }

        private static readonly QueueManager qm = QueueManager.Instance;
        
        protected BaseThread(String threadName, String queueName)
        {
            // Set our name
            ThreadName = threadName;

            logger.Trace(ThreadName + "|Starting");

            // Create our queue
            _myQueue = new BlockingQueue<ThreadMessage>();
            _myQueue.SetupQueueStats(queueName);

            // Name our queue statistics counter
            //_myQueue.SetupQueueStats(ThreadName + "Q");

            // Add it to the queuemanager so other threads can easily locate this queue
            qm.AddQueue(ref _myQueue, queueName);
            Qname = queueName;

            // Fire up the Runner method from the derived class
            ThreadStart threadStarter = Runner;
            _thisThread = new Thread(threadStarter);
        }

        public void Start()
        {
            _thisThread.Start();
        }

        protected bool TakeFunc(ThreadMessage tm, int i)
        {
            //_myQueue.DecrementQueueItemCounter();
            
            if ((tm.Cmd == Defines.ThreadExitMsg))
            {
                logger.Trace(ThreadName + "|Received Exit Command");
            }
            return (tm.Cmd != Defines.ThreadExitMsg);
        }
        protected abstract void Runner();
    }
}
