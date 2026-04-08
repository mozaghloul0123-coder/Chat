using Div.Link.Project01.BLL.Service;
using Div.Link.Project01.DAL.UnitOfWork;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Div.Link.Project01.BLL.AutoMapper;

namespace Div.Link.Project01.BLL
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IDoctorManager, DoctorManager>();
            services.AddScoped<IServicesAuth, ServicesAuth>();
            
            services.AddAutoMapper(typeof(MappingProfile));

            return services;
        }
    }
}
