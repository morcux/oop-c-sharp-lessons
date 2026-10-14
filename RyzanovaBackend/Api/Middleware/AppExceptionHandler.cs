using Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace Api.Middleware;

public class AppExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not AppException appEx)
        {
            return false;
        }

        httpContext.Response.StatusCode = appEx.StatusCode;
        await httpContext.Response.WriteAsJsonAsync(new { message = appEx.Message }, cancellationToken);
        return true;
    }
}
