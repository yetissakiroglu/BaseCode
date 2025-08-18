using System.ComponentModel.DataAnnotations;

namespace Economy.Panel.UI.Models.DatabaseBackupPageViewModels
{
    public class DatabaseBackupPageViewModel
    {
        // Form
        [Display(Name = "Yedek Klasörü")]
        [Required, StringLength(400)]
        public string TargetFolder { get; set; } = @"C:\SqlBackups"; // veya \\server\share

        [Display(Name = "Dosya Adı (.bak)")]
        [StringLength(260)]
        public string? FileName { get; set; } // boş bırakılırsa otomatik

        [Display(Name = "COPY_ONLY")]
        public bool CopyOnly { get; set; } = true; // diff zincirini etkilemez

        [Display(Name = "COMPRESSION")]
        public bool Compression { get; set; } = true;

        [Display(Name = "CHECKSUM")]
        public bool Checksum { get; set; } = true;

        [Display(Name = "RESTORE VERIFYONLY")]
        public bool Verify { get; set; } = true;

        // Ekran
        public string? LastResultMessage { get; set; }
        public string? LastBackupFullPath { get; set; }
        public List<FileItem> ExistingBackups { get; set; } = new();

        public class FileItem
        {
            public string Name { get; set; } = "";
            public long SizeBytes { get; set; }
            public DateTime LastWriteTime { get; set; }
        }
    }
}
