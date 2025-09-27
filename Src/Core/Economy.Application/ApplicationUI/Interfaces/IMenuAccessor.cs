using Economy.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Application.ApplicationUI.Interfaces
{
    public interface IMenuAccessor
    {
        Task<List<MenuItem>> GetAsync(string lang);
    }
}
