using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSC.Core.Writers
{
    public interface ICanSave<in T>
    {
        Task SaveOneAsync(T item);
        Task SaveManyAsync(IEnumerable<T> items);
    }
}
