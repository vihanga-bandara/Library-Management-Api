namespace Library.Backend.Application.Models
{
    public record UserReadingPaceSummaryDto(Guid UserId, string Name, double PagesPerDay) : UserDto(UserId, Name);
}