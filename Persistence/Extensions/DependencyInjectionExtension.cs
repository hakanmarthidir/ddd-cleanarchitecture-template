using Domain.Entities.UserAggregate;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Context.Relational;
using Persistence.Context.Relational.Uow;
using Persistence.Repository.User;
using System.Reflection;
using MediatR;

namespace Persistence.Extensions
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddDbContext<EFContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("TemplateConnection")));
            services.AddScoped<IUnitOfWork, EFUnitOfWork>();

            //REPOSITORIES ------            
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
