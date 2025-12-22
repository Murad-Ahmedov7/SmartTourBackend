using SmartTour.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTour.DataAccess.Repositories.Auth.Abstract
{
    public interface IUserRepository
    {
        // REGISTER
        Task AddAsync(User user);

        // LOGIN
        Task<User?> GetByEmailAsync(string email);
        //Task UpdateAsync(User user);

        // COMMON
        Task SaveChangesAsync();
        //GOT TOKEN
        Task<User> FindByResetTokenAsync(string token);
        Task<User?> GetByIdAsync(Guid id);


        Task<User?> GetByGoogleIdAsync(string googleId);
    }
}
