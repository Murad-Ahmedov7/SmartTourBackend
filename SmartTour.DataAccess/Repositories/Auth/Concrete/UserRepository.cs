using Microsoft.EntityFrameworkCore;
using SmartTour.DataAccess.Repositories.Auth.Abstract;
using SmartTour.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTour.DataAccess.Repositories.Auth.Concrete
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDataContext _context;


        public UserRepository(AppDataContext context)
        {
            _context = context;
        }

        // REGISTER
        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        //LOGIN
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        //// LOGIN (failed attempts, lockout, last login)
        //public Task UpdateAsync(User user)
        //{
        //    _context.Users.Update(user);
        //    return Task.CompletedTask;
        //}

        // COMMON
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
