﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Domain.Common
{
   
    public interface IDomainEvent
    {
     
        DateTime OccurredOn { get; }
    }
}
