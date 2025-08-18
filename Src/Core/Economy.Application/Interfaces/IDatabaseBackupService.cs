using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Application.Interfaces
{
    public interface IDatabaseBackupService
    {
        Task<(bool ok, string message, string? fullPath)> FullBackupAsync(
            string? dbName,
            string targetFolder,
            string? fileName,
            bool copyOnly,
            bool compression,
            bool checksum,
            bool verify,
            CancellationToken ct = default);
    }
}
