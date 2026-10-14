namespace Domain.Entities;

public enum PaymentStatus
{
    Pending,
    Success,
    Failed,
    Refunded
}

public class Payment : BaseEntity
{
    public string Phone { get; private set; }
    public PaymentStatus Status { get; private set; }

    public Guid ThemeId { get; private set; }
    public Theme Theme { get; private set; } = null!;

#pragma warning disable CS8618
    private Payment() { }
#pragma warning restore CS8618

    public Payment(Guid id, string phone, Guid themeId)
    {
        Id = id;
        Phone = phone;
        ThemeId = themeId;
        Status = PaymentStatus.Pending;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkSuccess()
    {
        Status = PaymentStatus.Success;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkFailed()
    {
        Status = PaymentStatus.Failed;
        UpdatedAt = DateTime.UtcNow;
    }
}