using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BosquesNalcahue.API.Auth
{
    public class ApiKeyAuthFilter(IConfiguration configuration) : IAuthorizationFilter
    {
        private readonly IConfiguration _configuration = configuration;

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string? providedApiKey = context.HttpContext.Request.Headers[ApiKeyConfig.ApiKeyHeader].FirstOrDefault();

            if (ValidateApiKey(providedApiKey) == false)
            {
                context.Result = new UnauthorizedObjectResult("Invalid API Key");
                return;
            }
        }

        private bool ValidateApiKey(string? providedApiKey)
        {
            if (string.IsNullOrEmpty(providedApiKey))
                return false;

            var validApiKey = _configuration.GetValue<string>(ApiKeyConfig.AuthSection);

            return providedApiKey.Equals(validApiKey, StringComparison.Ordinal);
            
        }
    }
}
