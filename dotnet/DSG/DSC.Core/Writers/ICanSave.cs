using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSC.Core.Writers
{
    public interface ICanSave<in T>
    {
        void SaveOne(T item);
        void SaveMany(IEnumerable<T> items);
    }
}
