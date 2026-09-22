namespace Application.DTOs;

public record CreateVideoRequest(
    string Title,
    string Url,
    int SequenceNumber,
    Guid ThemeId
);

public record UpdateVideoRequest(
    string Title,
    string Url,
    int SequenceNumber
);
