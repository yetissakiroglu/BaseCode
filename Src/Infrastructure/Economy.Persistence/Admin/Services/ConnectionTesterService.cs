using Economy.Application.AdminUI.Interfaces;
using Economy.Core.Interfaces;
using Economy.Core.Tools.Result;
using Economy.Domain.Entites.AdminEntity.EntityApp;
using Microsoft.Data.SqlClient;

namespace Economy.Persistence.Admin.Services
{
    public class ConnectionTesterService : IConnectionTesterService
    {
        private readonly IEntityRepository<App, int> _panelAppRepository;
        private readonly IUnitOfWork _unitOfWork;
        public ConnectionTesterService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _panelAppRepository = unitOfWork.DefaultEntityRepository<App>();
        }

        public async Task<ServiceResult<bool>> TestConnectionAsync(int appId)
        {

            try
            {
                var app = _panelAppRepository.GetForEdit(x=>x.Id == appId && x.IsDeleted== false);

                using (var connection = new SqlConnection(app.ConnectionString))
                {
                    await connection.OpenAsync();
                    var result = connection.State == System.Data.ConnectionState.Open;
                    if (result)
                        return ServiceResult<bool>.Success(result, "Bağlantı başarılı!");
                    else
                    {
                        return ServiceResult<bool>.Failure("Bağlantı başarısız!");
                    }
                }
            }
            catch (Exception ex)
            {
                return ServiceResult<bool>.Failure(
                     "Bağlantı başarısız!",
                     new[] { ex.Message }
                 );
            }
        }
    }
}
