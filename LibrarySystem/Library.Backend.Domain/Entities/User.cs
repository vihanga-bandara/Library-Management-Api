namespace Library.Backend.Domain.Entities;

public class User
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public ICollection<LoanTransaction> LoanTransactions { get; set; } = new List<LoanTransaction>();
}
