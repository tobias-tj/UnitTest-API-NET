using Api.Data;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDbContext _context;

        public UserRepository(UserDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateAsync(User user)
        {
            _context!.Users.Add(user);
            var result = await _context!.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var newUser = await _context!.Users.FirstOrDefaultAsync(_ => _.Id == id);
            if(newUser != null)
            {
                _context.Users.Remove(newUser);
                var result = await _context!.SaveChangesAsync();
                return result > 0;
            }
            return false;
        }

        public async Task<IEnumerable<User>> GetAllAsync() => await _context!.Users.ToListAsync();

        public async Task<User> GetByIdAsync(int id) => await _context!.Users!.FirstOrDefaultAsync(user => user.Id == id);

        public async Task<bool> UpdateAsync(User user)
        {
            var newUser = await _context.Users.FirstOrDefaultAsync(data => data.Id == user.Id);
            if(newUser != null)
            {
                newUser.Name = user.Name;
                newUser.Email = user.Email;
                var result = await _context!.SaveChangesAsync();
                return result > 0;
            }
            return false;
        }
    }
}
