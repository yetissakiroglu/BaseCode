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
       
        /// <summary>
        /// Autofac ContainerBuilder kullanarak LoggingDbContext ekler.
        /// </summary>
        public static void AddLoggingDbContextWithAutofac(this ContainerBuilder container, IConfiguration configuration)
        {
            container.Register(c =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<LoggingDbContext>();
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("LoggingDb"));
                return new LoggingDbContext(optionsBuilder.Options);
            }).InstancePerLifetimeScope();
        }


    }

}
