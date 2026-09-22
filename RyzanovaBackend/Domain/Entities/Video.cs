namespace Domain.Entities;

public class Video : BaseEntity
{
    public string Title { get; private set; }
    public string Url { get; private set; }
    public int SequenceNumber { get; private set; }

    public Guid ThemeId { get; private set; }

    public Theme Theme { get; private set; } = null!;

#pragma warning disable CS8618
    private Video() { }
#pragma warning restore CS8618

    public Video(Guid id, Guid themeId, string title, string url, int sequenceNumber)
    {
        Id = id;
        ThemeId = themeId;
        Title = title;
        Url = url;
        SequenceNumber = sequenceNumber;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Update(string title, string url, int sequenceNumber)
    {
        Title = title;
        Url = url;
        SequenceNumber = sequenceNumber;
        UpdatedAt = DateTime.UtcNow;
    }
}
