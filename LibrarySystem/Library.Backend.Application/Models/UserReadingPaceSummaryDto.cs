namespace Library.Backend.Application.Models
{
    public record UserReadingPaceSummaryDto(Guid UserId, string Name, double pagesPerDay) : UserDto(UserId, Name);
}