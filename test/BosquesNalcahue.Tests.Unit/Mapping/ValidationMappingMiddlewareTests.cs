using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;

namespace BosquesNalcahue.Tests.Unit.Mapping
{
    public class ValidationMappingMiddlewareTests
    {
        [Fact]
        public async Task InvokeAsync_WhenValidationExceptionIsThrown_ShouldSetStatusCodeTo400()
        {
            // Arrange
            var mockRequestDelegate = Substitute.For<RequestDelegate>();
            mockRequestDelegate
                .When(rd => rd(Arg.Any<HttpContext>()))
                .Do(callInfo => { throw new ValidationException(new[] { new ValidationFailure("TestProperty", "Test error message") }); });

            var middleware = new ValidationMappingMiddleware(mockRequestDelegate);

            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            Assert.Equal(400, context.Response.StatusCode);
        }

        [Fact]
        public async Task InvokeAsync_WhenValidationExceptionIsThrown_ShouldContainAppropriateMessage()
        {
            // Arrange
            var mockRequestDelegate = Substitute.For<RequestDelegate>();
            mockRequestDelegate
                .When(rd => rd(Arg.Any<HttpContext>()))
                .Do(callInfo => { throw new ValidationException(new[] { new ValidationFailure("TestProperty", "Test error message") }); });

            var middleware = new ValidationMappingMiddleware(mockRequestDelegate);

            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            // Act
            await middleware.InvokeAsync(context);
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(context.Response.Body);
            var responseBody = await reader.ReadToEndAsync();
            
            // Assert
            Assert.Contains("TestProperty", responseBody);
            Assert.Contains("Test error message", responseBody);
        }
    }
}
