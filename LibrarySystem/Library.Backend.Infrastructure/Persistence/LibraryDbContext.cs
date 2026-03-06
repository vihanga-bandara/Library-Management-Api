using Library.Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.Backend.Infrastructure.Persistence;

public class LibraryDbContext(DbContextOptions<LibraryDbContext> options) : DbContext(options)
{
    public DbSet<Book> Books => Set<Book>();
    public DbSet<User> Users => Set<User>();
    public DbSet<LoanTransaction> LoanTransactions => Set<LoanTransaction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>(entity =>
        {
            entity.Property(x => x.Title).IsRequired().HasMaxLength(255);
            entity.Property(x => x.PageCount).IsRequired();
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(x => x.Name).IsRequired().HasMaxLength(255);
        });

        modelBuilder.Entity<LoanTransaction>(entity =>
        {
            entity.Property(x => x.BorrowedAt).IsRequired();

            entity.HasOne(x => x.Book)
                .WithMany(b => b.LoanTransactions)
                .HasForeignKey(x => x.BookId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.User)
                .WithMany(u => u.LoanTransactions)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(x => x.BorrowedAt);
            entity.HasIndex(x => new { x.UserId, x.BookId });
        });

        base.OnModelCreating(modelBuilder);
    }
}
