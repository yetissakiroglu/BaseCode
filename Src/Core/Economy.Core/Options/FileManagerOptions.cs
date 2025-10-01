using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Core.Options
{
    public sealed class FileManagerOptions
    {
        public string RootPath { get; set; } = "wwwroot/uploads/images";
        public string PublicRequestPath { get; set; } = "/uploads/images";
        public int MaxUploadSizeMB { get; set; } = 10;
        public string[] AllowedExtensions { get; set; } = Array.Empty<string>();
        public ImageProcessingOptions ImageProcessing { get; set; } = new();
    }
}
