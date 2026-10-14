namespace Application.Exceptions;

public abstract class AppException(string message, int statusCode) : Exception(message)
{
    public int StatusCode { get; } = statusCode;
}

public class NotFoundException(string message = "Not found") : AppException(message, 404);

public class ForbiddenException(string message = "Forbidden") : AppException(message, 403);

public class BadRequestException(string message = "Bad request") : AppException(message, 400);
