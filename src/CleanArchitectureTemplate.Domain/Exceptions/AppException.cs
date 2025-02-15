namespace CleanArchitectureTemplate.Domain.Exceptions;

public class AppException(Error error) : Exception(error.Message)
{
    public ErrorType ErrorType { get; } = error.ErrorType;
}
