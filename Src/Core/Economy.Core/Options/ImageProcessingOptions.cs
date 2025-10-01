using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Core.Options
{
    public sealed class ImageProcessingOptions
    {
        public int MaxWidth { get; set; } = 2560;
        public bool CreateWebpCopy { get; set; } = true;
        public int WebpQuality { get; set; } = 80;
    }
}
