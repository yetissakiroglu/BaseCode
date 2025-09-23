using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Application.Interfaces
{
    public interface ICdnUrlService
    {
        /// <param name="path">"~/css/site.css" veya "/images/a.jpg" gibi</param>
        /// <param name="appendVersion">dosya tarihine göre ?v= ekle</param>
        string Url(string path, bool appendVersion = false);
    }
}
