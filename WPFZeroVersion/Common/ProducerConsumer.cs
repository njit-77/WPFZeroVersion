using System;
using System.Threading;
using System.Collections.Concurrent;

namespace WPFZeroVersion.Common
{
    /// <summary>简单生产者-消费者模式</summary>
    public class ProducerConsumer<T> where T : new()
    {
        /// <summary>队列</summary>
        private ConcurrentQueue<T> queue;

        /// <summary>信号量</summary>
        private readonly EventWaitHandle wait;

        /// <summary>消费者待执行任务</summary>
        private readonly Action<T> action;

        /// <summary>后台线程</summary>
        private readonly System.Threading.Thread thread;


        public ProducerConsumer(Action<T> _action)
        {
            queue = new ConcurrentQueue<T>();

            wait = new AutoResetEvent(false);

            action = _action;

            thread = new Thread(Work)
            {
                IsBackground = true
            };
            thread.Start();
        }

        private void Work()
        {
            while (true)
            {
                bool is_get_data = false;
                T? item = default;

                if (queue.Count > 0)
                {
                    is_get_data = queue.TryDequeue(out item);
                }

                if (is_get_data && item == null)
                {
                    /// 退出操作
                    return;
                }
                if (!is_get_data)
                {
                    wait.WaitOne();
                }
                else
                {
                    action(item);
                }
            }
        }

        public void AddT(T item)
        {
            queue.Enqueue(item);
            wait.Set();
        }
    }
}
