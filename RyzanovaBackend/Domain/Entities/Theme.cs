namespace Domain.Entities;

public enum ThemeType
{
    Lecture,
    Course
}

public class Theme : BaseEntity
{
    public string Name { get; private set; }
    public string Slug { get; private set; }
    public string Description { get; private set; }
    public ThemeType Type { get; private set; }
    public int Price { get; private set; }
    public bool IsActive { get; private set; }



    public Theme(Guid id, string name, string slug, string description, ThemeType type, int price)
    {
        Id = id;
        Name = name;
        Slug = slug;
        Description = description;
        Type = type;
        Price = price;
        IsActive = true;

        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }
#pragma warning disable CS8618
    private Theme() { }
#pragma warning restore CS8618

    private readonly List<Video> _videos = [];

    public IReadOnlyCollection<Video> Videos => _videos.AsReadOnly();

    public void AddVideo(Video video)
    {
        _videos.Add(video);
        UpdatedAt = DateTime.UtcNow;
    }
}
