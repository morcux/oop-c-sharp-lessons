namespace Application.DTOs;

public record VideoResponse(
    Guid Id,
    string Title,
    string Url,
    int SequenceNumber,
    Guid ThemeId
);
