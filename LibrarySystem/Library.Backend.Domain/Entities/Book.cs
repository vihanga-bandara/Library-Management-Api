namespace Library.Backend.Domain.Entities;

public class Book
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public int PageCount { get; set; }
}
