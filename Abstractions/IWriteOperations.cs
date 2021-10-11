using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstractions
{
    //interfata generica: <T>
    public interface IWriteOperations<T>
    {
        public void PerformOperation(T operation);
    }
}
