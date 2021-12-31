using AspectCore.DynamicProxy;
using DDDTemplate.Application.Abstraction.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDTemplate.Application.Abstraction.Attributes
{
    //to create a global interceptor you should use AbstractInterceptor and configure it in Startup/ConfigureServices 
    //use this already commented line -> services.ConfigureDynamicProxy(config => config.Interceptors.AddTyped<T>());

    public class PerformanceAttribute : AbstractInterceptorAttribute
    {
        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            var _logger = context.ServiceProvider.GetService<ILogService<PerformanceAttribute>>();
            var stopwatch = Stopwatch.StartNew();
            await next(context).ConfigureAwait(false);
            stopwatch.Stop();
            var method = context.ImplementationMethod;
            _logger.LogInformation($"PERFORMANCE {method.DeclaringType.Namespace} {method.DeclaringType.Name}.{method.Name} {method.DeclaringType.Assembly.GetName().Name} {stopwatch.ElapsedMilliseconds} ms ");
        }
    }
}
