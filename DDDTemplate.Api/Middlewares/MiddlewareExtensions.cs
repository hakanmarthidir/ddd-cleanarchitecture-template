using Microsoft.AspNetCore.Builder;

namespace DDDTemplate.Api.Middlewares
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseJwtMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<JwtMiddleware>();
        }

        public static IApplicationBuilder UseCorrelationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CorrelationMiddleware>();
        }
    }
}
