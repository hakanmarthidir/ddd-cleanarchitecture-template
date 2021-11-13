using DDDTemplate.Api.ActionFilters;
using DDDTemplate.Api.Middlewares;
using DDDTemplate.Application.Contracts.Shared;
using DDDTemplate.Application.Extensions;
using DDDTemplate.Application.User.Config;
using DDDTemplate.Infrastructure.Notification.Config;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

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
            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
            services.Configure<MailGunConfig>(Configuration.GetSection("EmailConfig"));
            services.Configure<JwtConfig>(Configuration.GetSection("JwtConfig"));

            //Choose Database Context
            //--------MySql MariaDb-------------
            //services.AddMySqlDatabaseContext(Configuration);

            //---------MongoDB------------
            services.AddMongoDatabaseContext();

            //Implementation Of Repositories and Services
            services.AddRepositories();
            services.AddServices();
            //---------------------

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
                    fv.RegisterValidatorsFromAssemblyContaining<IDataValidator>();

                });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DDDTemplate.Api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseExceptionHandler("/error-local-development");
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DDDTemplate.Api v1"));
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

            // custom jwt auth middleware
            app.UseMiddleware<JwtMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
