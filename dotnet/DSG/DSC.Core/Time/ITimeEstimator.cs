using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSC.Core.Time
{
    public interface ITimeEstimator : IDisposable
    {
        void ExecuteItem(Action item);
        Tuple<long, int, int> EstimateLeftTime();
    }
}
