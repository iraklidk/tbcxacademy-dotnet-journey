using System.Globalization;

namespace UserManagementAPI.infrastructure.middlewares
{
    public class RequestCultureMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestCultureMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var cultureHeader = context.Request.Headers["Accept-Language"].ToString();
            if (!string.IsNullOrEmpty(cultureHeader))
            {
                try
                {
                    var culture = new CultureInfo(cultureHeader);
                    CultureInfo.CurrentCulture = culture;
                    CultureInfo.CurrentUICulture = culture;
                }
                catch { }
            }
            await _next(context);
        }
    }
}
