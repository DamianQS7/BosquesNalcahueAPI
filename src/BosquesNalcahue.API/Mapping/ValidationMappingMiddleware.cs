using BosquesNalcahue.Contracts.Responses;
using FluentValidation;

namespace BosquesNalcahue.API.Mapping
{
    public class ValidationMappingMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                context.Response.StatusCode = 400;
                var validationErrorResponse = new ValidationErrorResponse
                {
                    Errors = ex.Errors.Select(e => new ValidationResponse(e.PropertyName, e.ErrorMessage))
                };

                await context.Response.WriteAsJsonAsync(validationErrorResponse);
            }
        }
    }
}
