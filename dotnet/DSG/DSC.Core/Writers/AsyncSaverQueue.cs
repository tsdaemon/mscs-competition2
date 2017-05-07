using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace DSC.Core.Writers
{
    public class AsyncSaverQueue<T>
    {
        //private static readonly Object obj = new object();
        private Thread _thread;

        private ConcurrentQueue<T> _queue;
        private ICanSave<T> _saver;

        public AsyncSaverQueue(ICanSave<T> saver)
        {
            _saver = saver;
            _queue = new ConcurrentQueue<T>();

            _thread = new Thread(async () => await Go());
            _thread.Start();
        }

        public void Save(T item)
        {
            _queue.Enqueue(item);
        }

        private async Task Go()
        {
            while (true)
            {
                var next = GetNext();

                await _saver.SaveOneAsync(next);
            }
        }

        private T GetNext()
        {
            while (_queue.IsEmpty)
            {
                Thread.Sleep(100);
            }

            while (true)
            {
                T item;
                if (_queue.TryDequeue(out item))
                {
                    return item;
                }
                
                if (_queue.IsEmpty)
                    Thread.Sleep(100);
            }
        }
    }
}
