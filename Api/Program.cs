using Api.ActionFilters;
using Api.Middlewares;
using Application.Contracts.Auth.Validations;
using Application.Extensions;
using FluentValidation.AspNetCore;
using Infrastructure.Extensions;
using Persistence.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

//SERILOG CONF BEGIN ----
builder.Logging.ClearProviders();
var logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    //.Enrich.FromLogContext()
    //.Enrich.WithExceptionDetails()
    //.Enrich.WithMachineName()
    .WriteTo.Console()
    .WriteTo.Debug()
    //.WriteTo.Elasticsearch(ConfigureElasticSink(builder.Configuration, builder.Environment))
    .CreateLogger();
builder.Logging.AddSerilog(logger);
//SERILOG CONF END -----

builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration, builder.Environment);
builder.Services.AddServices();

//builder.Services.ConfigureDynamicProxy(config => config.Interceptors.AddTyped<PerformanceAttribute>()); // AspectCore - Global Interceptor

builder.Services.AddCors();
builder.Services.AddControllers(opt =>
{
    opt.Filters.Add(typeof(ValidatorActionFilter));
}).AddNewtonsoftJson()
.AddFluentValidation(fv =>
                {
                    fv.ImplicitlyValidateChildProperties = true;
                    fv.ImplicitlyValidateRootCollectionElements = true;
                    fv.RegisterValidatorsFromAssemblyContaining<ForgotPasswordValidator>();

                }).AddControllersAsServices();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error-local-development");
    app.UseSwagger();
    app.UseSwaggerUI();
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

app.Run();
