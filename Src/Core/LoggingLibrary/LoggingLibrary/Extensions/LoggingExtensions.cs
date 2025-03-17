using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;
using LoggingLibrary.Data;
using LoggingLibrary.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace LoggingLibrary.Extensions
{
  
        public static class LoggingExtensions
        {
              public static void AddLoggingDbContext(this IServiceCollection services, IConfiguration configuration)
            {
            // 📌 LoggingDbContext'i servis olarak kaydet
            services.AddDbContext<LoggingDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("LoggingDb")));




        }
    }
    
}
