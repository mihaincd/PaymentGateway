﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstractions
{
    //interfata generica: <T>
    public interface IWriteOperations<TCommand>
    {
        public void PerformOperation(TCommand operation);
    }
}
