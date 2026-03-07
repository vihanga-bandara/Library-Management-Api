using Library.Backend.Domain.Entities;

namespace Library.Backend.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserById(Guid userId);
    }
}
