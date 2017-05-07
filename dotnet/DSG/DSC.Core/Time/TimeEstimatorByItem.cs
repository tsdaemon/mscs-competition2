using System;
using System.Collections.Concurrent;
using System.Linq;

namespace DSC.Core.Time
{
    public class TimeEstimatorByItem : ITimeEstimator
    {
        private readonly int _items;

        private BlockingCollection<long> _executed = new BlockingCollection<long>();

        public TimeEstimatorByItem(int items)
        {
            _items = items;
        }

        public void ExecuteItem(Action item)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            item();
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            _executed.Add(elapsedMs);
        }

        public Tuple<long, int, int> EstimateLeftTime()
        {
            var all = _items;
            var processed = _executed.Count();
            if(processed == 0) return new Tuple<long, int, int>(0, processed, all);

            var meanTimeByItem = _executed.Sum() / processed;

            var left = all - processed;
            var leftTime = left * meanTimeByItem;

            return new Tuple<long, int, int>(leftTime, processed, all);
        }

        public void Dispose()
        {
            
        }
    }
}
