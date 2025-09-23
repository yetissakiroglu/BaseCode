using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Core.Enums
{
    [Flags]
    public enum CustomScriptPlacement
    {
        None = 0,
        Header = 1,   // <head>
        Footer = 2    // </body> öncesi
    }
}
