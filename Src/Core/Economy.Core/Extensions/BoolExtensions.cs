using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Core.Extensions
{
    public static class BoolExtensions
    {
        public static string ToTurkishYesNo(this bool value)
        {
            return value ? "Evet" : "Hayır";
        }
    }
}
