using AspectCore.DynamicProxy;
using AspectCore.DynamicProxy.Parameters;
using DDDTemplate.Application.Abstraction.Interfaces;
using DDDTemplate.Application.Abstraction.Response;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDTemplate.Application.Abstraction.Attributes
{
    public class LogAttribute : AbstractInterceptorAttribute
    {
        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            var isAsync = context.IsAsync();
            var _logger = context.ServiceProvider.GetService<ILogService<LogAttribute>>();
            _logger.LogInformation($"REQUEST : {context.ImplementationMethod.DeclaringType.Name} - {this.GetParametersAsString(context.Parameters)}");

            await next.Invoke(context).ConfigureAwait(false);                  
            
            var returnValue = isAsync == true ? (await context.UnwrapAsyncReturnValue()) : (context.ReturnValue);            
            var serializedReturnValue = this.JsonSerialize(returnValue);
            _logger.LogInformation($"RESPONSE : {serializedReturnValue}");
        }

        private string GetParametersAsString(object[] parameters)
        {
            StringBuilder builder = new StringBuilder();
            if (parameters != null && parameters.Length > 0)
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    var parameter = parameters[i];
                    var serialized = this.JsonSerialize(parameter);
                    if (i != 0)
                        builder.Append(" - ");

                    builder.Append(serialized);
                }                
            }
            return builder.ToString();
        }

        private string JsonSerialize(object value)
        {
            return JsonConvert.SerializeObject(value);
        }
    }   
}
