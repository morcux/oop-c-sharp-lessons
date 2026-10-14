namespace Application.DTOs;

public record CreatePhotoRequest(
    string Url,
    int SequenceNumber,
    Guid ThemeId
);

public record UpdatePhotoRequest(
    string Url,
    int SequenceNumber
);
