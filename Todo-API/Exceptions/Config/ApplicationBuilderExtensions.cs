using ErrorManagement.Configurations;

namespace Todo_API.Exceptions.Config
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder AddGlobalErrorHandler(this IApplicationBuilder applicationBuilder)
            => applicationBuilder.UseMiddleware<GlobalErrorHandlingMiddleware>();
    }
}
