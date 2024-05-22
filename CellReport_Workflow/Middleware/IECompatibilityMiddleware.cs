namespace CellReport_Workflow.Middleware
{
    public class IECompatibilityMiddleware
    {
        private readonly RequestDelegate _next;

        public IECompatibilityMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            context.Response.Headers.Add("X-UA-Compatible", "IE=edge");
            await _next(context);
        }
    }
}
