using AspectCore.Configuration;
using AspectCore.Extensions.DependencyInjection;
using DDDTemplate.Api.ActionFilters;
using DDDTemplate.Api.Middlewares;
using DDDTemplate.Application.Abstraction.Attributes;
using DDDTemplate.Application.Contracts.Auth.Validations;
using DDDTemplate.Application.Extensions;
using DDDTemplate.Infrastructure.Extensions;
using DDDTemplate.Persistence.Extensions;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DDDTemplate.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            HostingEnvironment = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment HostingEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddPersistence(Configuration);
            services.AddInfrastructure(Configuration, HostingEnvironment);
            services.AddServices();
            //services.ConfigureDynamicProxy(config => config.Interceptors.AddTyped<PerformanceAttribute>()); // AspectCore - Global Interceptor
            services.AddCors();
            services.AddControllers(opt =>
                {
                    opt.Filters.Add(typeof(ValidatorActionFilter));
                })
                .AddNewtonsoftJson()
                .AddFluentValidation(fv =>
                {
                    fv.ImplicitlyValidateChildProperties = true;
                    fv.ImplicitlyValidateRootCollectionElements = true;
                    fv.RegisterValidatorsFromAssemblyContaining<ForgotPasswordValidator>();

                })
                .AddControllersAsServices()
                ;

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseExceptionHandler("/error-local-development");
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            app.UseRouting();

            app.UseCors(x => x
               .AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());

            // custom middlewares            
            app.UseCorrelationMiddleware();            
            app.UseJwtMiddleware();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
