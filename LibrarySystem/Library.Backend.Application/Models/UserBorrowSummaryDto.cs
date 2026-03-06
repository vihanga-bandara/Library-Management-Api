namespace Library.Backend.Application.Models
{
    public record UserBorrowSummaryDto(Guid UserId, string Name, int BorrowedCount) : UserDto(UserId, Name);
}