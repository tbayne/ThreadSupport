using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ThreadSupport
{

    public class BlockingQueue<T> : IEnumerable<T>
    {
        public int Count { get; private set; }
        //private Queue<T> _queue = new Queue<T>();
        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        private BlockingCollection<T> _queue = new BlockingCollection<T>();
        public T Dequeue()
        {
            T result;
            _queue.TryTake(out result);
            return result;
        }

        public void Enqueue(T data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            _queue.TryAdd(data);
            Count++;
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