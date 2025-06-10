using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Data;
using WalletAppication.Interfaces;
using WalletAppication.Services;

namespace WalletAppication.Attributes
{
    public class RateLimitAttribute : ActionFilterAttribute
    {
        private readonly string _endpoint;

        public RateLimitAttribute(string endpoint)
        {
            _endpoint = endpoint;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var clientIp = context.HttpContext.Connection.RemoteIpAddress?.ToString();
            var _rateLimiterService = context.HttpContext.RequestServices.GetRequiredService<IRateLimiterService>();
            if (string.IsNullOrWhiteSpace(clientIp) || _rateLimiterService.IsRateLimited(clientIp, _endpoint))
            {
                context.Result = new StatusCodeResult(429); // Too Many Requests
                return;
            }

            await base.OnActionExecutionAsync(context, next); // Continue execution if allowed
        }
    }
}
