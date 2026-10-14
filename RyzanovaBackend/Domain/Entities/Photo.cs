namespace Domain.Entities;

public class Photo : BaseEntity
{
    public string Url { get; private set; }
    public int SequenceNumber { get; private set; }

    public Guid ThemeId { get; private set; }

    public Theme Theme { get; private set; } = null!;

#pragma warning disable CS8618
    private Photo() { }
#pragma warning restore CS8618

    public Photo(Guid id, Guid themeId, string url, int sequenceNumber)
    {
        Id = id;
        ThemeId = themeId;
        Url = url;
        SequenceNumber = sequenceNumber;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Update(string url, int sequenceNumber)
    {
        Url = url;
        SequenceNumber = sequenceNumber;
        UpdatedAt = DateTime.UtcNow;
    }
}
