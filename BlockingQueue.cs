using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Metrics;

namespace ThreadSupport
{

    public class BlockingQueue<T> : IEnumerable<T>
    {
        public int Count { get; private set; }
        //private Queue<T> _queue = new Queue<T>();
        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        private BlockingCollection<T> _queue = new BlockingCollection<T>();
        // Setup our Stats
        private bool _enableStats = false;

        private Counter _inBoundMessagesReceived;
        private Meter _inBoundMsgsPerSecond;
        private Counter _outBoundMessagesReceived;
        private Meter _outBoundMsgsPerSecond;

        public void SetupQueueStats(string QueueStatsName)
        {
        _inBoundMessagesReceived = Metric.Counter(QueueStatsName + "Received Messages", Unit.Custom("Incoming Messages"));
        _inBoundMsgsPerSecond = Metric.Meter(QueueStatsName + "Inbound MPS", Unit.Items, TimeUnit.Seconds);
        _outBoundMessagesReceived = Metric.Counter(QueueStatsName + "Processed Messages", Unit.Custom("Incoming Messages"));
        _outBoundMsgsPerSecond = Metric.Meter(QueueStatsName + " Processed MPS", Unit.Items, TimeUnit.Seconds);
        _enableStats = true;
        }

    public bool TryTake(int millisecondsTimeout, out T item)
        {
            var result = _queue.TryTake(out item, millisecondsTimeout);
            return result;
        }

        public T Dequeue()
        {
            T result;
            _queue.TryTake(out result);
            if (_enableStats)
            {
                _outBoundMessagesReceived.Increment();
                _outBoundMsgsPerSecond.Mark();
            }
            return result;
        }

        public void Enqueue(T data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            _queue.TryAdd(data);
            Count++;
            if (_enableStats)
            {
                _inBoundMessagesReceived.Increment();
                _inBoundMsgsPerSecond.Mark();
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _queue.GetConsumingEnumerable().GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }
}