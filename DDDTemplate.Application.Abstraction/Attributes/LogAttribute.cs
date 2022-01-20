using AspectCore.DynamicProxy;
using DDDTemplate.Application.Abstraction.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DDDTemplate.Application.Abstraction.Attributes
{
    public class LogAttribute : AbstractInterceptorAttribute
    {
        private bool _hasSensitiveData = false;
        public LogAttribute(bool hasSensitiveData)
        {
            this._hasSensitiveData = hasSensitiveData;
        }

        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            var isAsync = context.IsAsync();
            var _logger = context.ServiceProvider.GetService<ILogService<LogAttribute>>();
            var methodName = context.ImplementationMethod.DeclaringType.Name; 
              _logger.LogInformation($"REQUEST : {methodName} - {this.GetParametersAsString(context.Parameters)}");

            await next.Invoke(context).ConfigureAwait(false);

            var returnValue = isAsync == true ? (await context.UnwrapAsyncReturnValue()) : (context.ReturnValue);
            var serializedReturnValue = this.JsonSerialize(returnValue);
            _logger.LogInformation($"RESPONSE : {methodName} - {serializedReturnValue}");
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
            var serializedData = JsonConvert.SerializeObject(value);
            if (this._hasSensitiveData)
                serializedData = this.JsonMask(serializedData);

            return serializedData;
        }

        private string JsonMask(string data)
        {
            var blacklist = new string[] { "Password", "Email", "PasswordConfirm", "Token" };
            var mask = "****";
            var maskedValue = data;

            foreach (var item in blacklist)
            {
                var pattern = $"(?<=\"{item}\":\")[^\"]+(?=\")";
                maskedValue = Regex.Replace(maskedValue, pattern, mask);
            }
            return maskedValue;
        }
    }
}
