namespace Library.Backend.Application.Models
{
    public record BorrowedBooksDto(Guid Id, string Title, int BookCount) : BookDto(Id, Title);
}
