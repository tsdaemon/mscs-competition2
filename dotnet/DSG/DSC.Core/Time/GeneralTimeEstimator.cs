using System;
using System.Diagnostics;
using System.Threading;

namespace DSC.Core.Time
{
    public class GeneralTimeEstimator : ITimeEstimator
    {
        private readonly int _items;
        private int _processed = 0;
        private bool started;
        private Stopwatch _watch;

        public GeneralTimeEstimator(int items)
        {
            _items = items;
        }

        public void ExecuteItem(Action item)
        {
            if (!started)
            {
                _watch = Stopwatch.StartNew();
                started = true;
            }


            item();

            Interlocked.Increment(ref _processed);
        }

        public Tuple<long, int, int> EstimateLeftTime()
        {
            if(!started) return new Tuple<long, int, int>(0, 0, _items);

            var all = _items;
            var processed = _processed;

            if (processed == 0) return new Tuple<long, int, int>(0, 0, _items);

            var meanTimeByItem = _watch.ElapsedMilliseconds / processed;

            var left = all - processed;
            var leftTime = left * meanTimeByItem;

            return new Tuple<long, int, int>(leftTime, processed, all);
        }

        public void Dispose()
        {
            _watch?.Stop();
        }
    }
}
