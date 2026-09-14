namespace OfflineServiceDesk.Core;

public sealed class Ticket
{
    public Ticket(Guid id, string title, string status, DateTimeOffset updatedAtUtc)
    {
        Id = id;
        Title = NormalizeTitle(title);
        Status = status;
        UpdatedAtUtc = updatedAtUtc;
    }

    public Guid Id { get; }
    public string Title { get; private set; }
    public string Status { get; }
    public DateTimeOffset UpdatedAtUtc { get; private set; }

    public void Rename(string? title, DateTimeOffset nowUtc)
    {
        Title = NormalizeTitle(title);
        UpdatedAtUtc = nowUtc;
    }

    private static string NormalizeTitle(string? title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new DomainException("Il titolo è obbligatorio.");
        }

        string result = title.Trim();
        return result.Length <= 120
            ? result
            : throw new DomainException("Il titolo supera 120 caratteri.");
    }
}

public sealed class DomainException(string message) : Exception(message);