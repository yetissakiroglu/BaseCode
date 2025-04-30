using Economy.Base.Persistence.Providers;
using Microsoft.AspNetCore.Mvc;

namespace Economy.Panel.UI.Controllers
{
    public class MigrationController : Controller
    {
        private readonly MigrationService _migrationService;

        public MigrationController(MigrationService migrationService)
        {
            _migrationService = migrationService;
        }

        [HttpGet("migrate/{tenantId}")]
        public async Task<IActionResult> MigrateTenant(int tenantId)
        {
            await _migrationService.MigrateTenantAsync(tenantId);
            return Ok("Tenant veritabanı migrasyonu başarıyla tamamlandı.");
        }

        [HttpGet("migrate-master")]
        public async Task<IActionResult> MigrateMasterDb()
        {
            await _migrationService.MigrateMasterDbAsync();
            return Ok("Master veritabanı migrasyonu başarıyla tamamlandı.");
        }


        [HttpGet("migrate-hotels")]
        public async Task<IActionResult> MigrateAllHotelDbs()
        {
            await _migrationService.MigrateAllHotelDatabasesAsync();
            return Ok("Tüm otel veritabanlarına migration uygulandı.");
        }

    }
}
