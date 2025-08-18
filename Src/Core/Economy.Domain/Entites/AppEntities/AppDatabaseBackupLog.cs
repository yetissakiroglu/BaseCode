using Economy.Domain.BaseEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Domain.Entites.AppEntities
{
    [Table("AppDatabaseBackupLogs")]
    public class AppDatabaseBackupLog : BaseEntity<int>
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [StringLength(128)] public string DatabaseName { get; set; } = "";
        [StringLength(400)] public string TargetFolder { get; set; } = "";
        [StringLength(260)] public string FileName { get; set; } = "";
        [StringLength(600)] public string FullPath { get; set; } = "";

        public bool CopyOnly { get; set; }
        public bool Compression { get; set; }
        public bool Checksum { get; set; }
        public bool Verify { get; set; }

        public bool Succeeded { get; set; }
        public int? DurationMs { get; set; }
        public long? FileSizeBytes { get; set; }           // erişebilirsek doldururuz
        [StringLength(1024)] public string? ErrorMessage { get; set; }

        public int? UserId { get; set; }
        [StringLength(256)] public string? UserName { get; set; }
    }
}
