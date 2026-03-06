namespace Library.Backend.Domain.Entities;

public class LoanTransaction
{
    public Guid Id { get; set; }

    public Guid BookId { get; set; }

    public Book? Book { get; set; }

    public Guid UserId { get; set; }

    public User? User { get; set; }

    public DateTime BorrowedAt { get; set; }

    public DateTime? ReturnedAt { get; set; }
}
