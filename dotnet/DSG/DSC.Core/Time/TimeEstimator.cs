using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSC.Core.Time
{
    public class TimeEstimator
    {
        private readonly int _items;

        private BlockingCollection<int> _executed = new BlockingCollection<int>();

        public TimeEstimator(int items)
        {
            _items = items;
        }

        public void ExecuteItem(Action item)
        {
            StopWatch
        }
    }
}
