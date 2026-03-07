using Library.Backend.Application.Interfaces;
using Library.Backend.Domain.Entities;
using Library.Backend.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Library.Backend.Infrastructure.Repositories
{
    internal class UserRepository : IUserRepository
    {
        private readonly LibraryDbContext _dbContext;

        public UserRepository(LibraryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User?> GetUserById(Guid userId)
        {
            return await _dbContext.Users
                .AsNoTracking()
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync();
        }
    }
}
