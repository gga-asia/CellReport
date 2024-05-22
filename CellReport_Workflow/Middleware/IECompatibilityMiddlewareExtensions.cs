namespace CellReport_Workflow.Middleware
{
    public static class IECompatibilityMiddlewareExtensions
    {
        public static IApplicationBuilder UseIeCompatibility(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<IECompatibilityMiddleware>();
        }
    }
}
