namespace CleanArchitectureTemplate.Domain;

public sealed record Error(
    string Message,
    ErrorType ErrorType
);
