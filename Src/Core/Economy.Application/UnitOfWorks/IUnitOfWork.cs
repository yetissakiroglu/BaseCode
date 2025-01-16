using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Application.UnitOfWorks
{
    public interface IUnitOfWork
    {
        Task<int> CommitAsync();
    }
}
