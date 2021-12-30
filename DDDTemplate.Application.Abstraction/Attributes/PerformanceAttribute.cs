using AspectCore.DynamicProxy;
using DDDTemplate.Application.Abstraction.Interfaces;
using System;
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
            var stopwatch = Stopwatch.StartNew();
            await next(context).ConfigureAwait(false);
            stopwatch.Stop();
            Trace.WriteLine($"PERFORMANCE of Executed method {context.ImplementationMethod.DeclaringType.Namespace} " +
                $"{context.ImplementationMethod.DeclaringType.Name}.{context.ImplementationMethod.Name} " +
                $" ({context.ImplementationMethod.DeclaringType.Assembly.GetName().Name}) in " +
                $"{stopwatch.ElapsedMilliseconds} ms "
            );
        }
    }    
}
