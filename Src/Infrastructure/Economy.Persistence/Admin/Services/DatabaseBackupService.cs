using Economy.Application.AdminUI.Interfaces;
using Economy.Core.Interfaces;
using Economy.Domain.Entites.AdminEntity.EntityApp;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Economy.Persistence.Admin.Services
{

    public class DatabaseBackupService : IDatabaseBackupService
    {
        private readonly string _connStr;
        private readonly IUnitOfWork _uow;
        private readonly IEntityRepository<AppDatabaseBackupLog, int> _repo;

        public DatabaseBackupService(IConfiguration cfg, IUnitOfWork uow)
        {
            _connStr = cfg.GetConnectionString("DefaultConnection")
                       ?? throw new InvalidOperationException("Connection string not found");
            _uow = uow;
            _repo = uow.DefaultEntityRepository<AppDatabaseBackupLog>();
        }

        public async Task<(bool ok, string message, string? fullPath)> FullBackupAsync(
            string? dbName, string targetFolder, string? fileName,
            bool copyOnly, bool compression, bool checksum, bool verify,
            CancellationToken ct = default)
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();

            var cnStrBuilder = new SqlConnectionStringBuilder(_connStr);
            var databaseName = string.IsNullOrWhiteSpace(dbName) ? cnStrBuilder.InitialCatalog : dbName.Trim();

            var safeDb = databaseName.Replace(' ', '_');
            var name = string.IsNullOrWhiteSpace(fileName) ? $"{safeDb}_{DateTime.UtcNow:yyyyMMdd_HHmmss}.bak" : fileName.Trim();
            if (!name.EndsWith(".bak", StringComparison.OrdinalIgnoreCase)) name += ".bak";

            var sep = targetFolder.EndsWith("\\") || targetFolder.EndsWith("/") ? "" : "\\";
            var fullPath = $"{targetFolder}{sep}{name}";

            // BACKUP komutu hazırlanıyor (önceki kod gibi)...
            var options = new List<string> { "INIT", "STATS = 10" };
            if (copyOnly) options.Add("COPY_ONLY");
            if (compression) options.Add("COMPRESSION");
            if (checksum) options.Add("CHECKSUM");

            var backupSql = $@"
BACKUP DATABASE [{databaseName}]
TO DISK = @path
WITH {string.Join(", ", options)};";

            var verifySql = checksum && verify
                ? "RESTORE VERIFYONLY FROM DISK = @path WITH CHECKSUM;"
                : verify ? "RESTORE VERIFYONLY FROM DISK = @path;" : null;

            bool ok = false;
            string message;
            long? size = null;

            try
            {
                using var cn = new SqlConnection(_connStr);
                await cn.OpenAsync(ct);

                using (var cmd = new SqlCommand(backupSql, cn))
                {
                    cmd.Parameters.Add("@path", SqlDbType.NVarChar, 4000).Value = fullPath;
                    cmd.CommandTimeout = 0;
                    await cmd.ExecuteNonQueryAsync(ct);
                }

                if (verifySql != null)
                {
                    using var cmd2 = new SqlCommand(verifySql, cn);
                    cmd2.Parameters.Add("@path", SqlDbType.NVarChar, 4000).Value = fullPath;
                    cmd2.CommandTimeout = 0;
                    await cmd2.ExecuteNonQueryAsync(ct);
                }

                // Opsiyonel: Web sunucusu bu yola erişebiliyorsa dosya boyutunu al
                try
                {
                    var fi = new FileInfo(fullPath);
                    if (fi.Exists) size = fi.Length;
                }
                catch { }

                ok = true;
                message = $"Yedekleme tamamlandı: {fullPath}";
                return (ok, message, fullPath);
            }
            catch (Exception ex)
            {
                message = $"Yedekleme hatası: {ex.Message}";
                return (ok, message, null);
            }
            finally
            {
                sw.Stop();

                // LOG KAYDI
                var log = new AppDatabaseBackupLog
                {
                    CreatedAt = DateTime.UtcNow,
                    DatabaseName = databaseName,
                    TargetFolder = targetFolder,
                    FileName = name,
                    FullPath = fullPath,
                    CopyOnly = copyOnly,
                    Compression = compression,
                    Checksum = checksum,
                    Verify = verify,
                    Succeeded = ok,
                    DurationMs = (int)sw.ElapsedMilliseconds,
                    FileSizeBytes = size,
                    // Replace this line:

                    // doto:    
                    //ErrorMessage = ok ? null : message,
                    // İstersen HttpContext’ten kullanıcı bilgisi geçirip doldurabilirsin
                };

                _repo.Add(log);
                await _uow.SaveDefaultChangesAsync();
            }
        }
    }
}
