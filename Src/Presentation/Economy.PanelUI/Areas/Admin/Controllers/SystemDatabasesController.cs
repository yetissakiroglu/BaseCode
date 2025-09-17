using Economy.Application.Interfaces;
using Economy.Panel.UI.Models.DatabaseBackupPageViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Economy.Panel.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    [Route("[area]/[controller]")]
    public class SystemDatabasesController : Controller
    {
        private readonly IDatabaseBackupService _backup;

        public SystemDatabasesController(IDatabaseBackupService backup)
        {
            _backup = backup;
        }

        [HttpGet("Backup")]
        public IActionResult Backup()
        {
            var vm = new DatabaseBackupPageViewModel();

            // Not: Bu listeleme web sunucusunun diskini okur. SQL Server farklı makinedeyse boş olabilir.
            if (Directory.Exists(vm.TargetFolder))
            {
                try
                {
                    vm.ExistingBackups = Directory.GetFiles(vm.TargetFolder, "*.bak")
                        .Select(f => {
                            var fi = new FileInfo(f);
                            return new DatabaseBackupPageViewModel.FileItem
                            {
                                Name = fi.Name,
                                SizeBytes = fi.Length,
                                LastWriteTime = fi.LastWriteTime
                            };
                        })
                        .OrderByDescending(x => x.LastWriteTime)
                        .Take(50)
                        .ToList();
                }
                catch { /* paylaşıma erişim yoksa boş geç */ }
            }

            return View(vm);
        }

        [HttpPost("Backup")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Backup(DatabaseBackupPageViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var (ok, message, fullPath) = await _backup.FullBackupAsync(
                dbName: null,
                targetFolder: vm.TargetFolder,
                fileName: vm.FileName,
                copyOnly: vm.CopyOnly,
                compression: vm.Compression,
                checksum: vm.Checksum,
                verify: vm.Verify);

            vm.LastResultMessage = message;
            vm.LastBackupFullPath = fullPath;

            // Listeyi yenile (sunucu erişebiliyorsa)
            vm.ExistingBackups.Clear();
            if (Directory.Exists(vm.TargetFolder))
            {
                try
                {
                    vm.ExistingBackups = Directory.GetFiles(vm.TargetFolder, "*.bak")
                        .Select(f => {
                            var fi = new FileInfo(f);
                            return new DatabaseBackupPageViewModel.FileItem
                            {
                                Name = fi.Name,
                                SizeBytes = fi.Length,
                                LastWriteTime = fi.LastWriteTime
                            };
                        })
                        .OrderByDescending(x => x.LastWriteTime)
                        .Take(50)
                        .ToList();
                }
                catch { /* erişim yoksa boş */ }
            }

            if (!ok) ModelState.AddModelError(string.Empty, message);
            return View(vm);
        }

        // (opsiyonel) Download — yalnızca web sunucusundan okunabilirse çalışır
        [HttpGet("Backup/Download")]
        public IActionResult Download([FromQuery] string file, [FromQuery] string dir)
        {
            if (string.IsNullOrWhiteSpace(file) || string.IsNullOrWhiteSpace(dir)) return BadRequest();
            var path = Path.Combine(dir, file);
            if (!System.IO.File.Exists(path)) return NotFound();
            return PhysicalFile(path, "application/octet-stream", file);
        }
    }

}
