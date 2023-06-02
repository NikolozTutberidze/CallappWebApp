using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.Abstracts
{
    public interface IUserRepository
    {
        Task AddUserAsync(User user);
        Task<User> GetUserByIdAsync(int id);
        Task<ICollection<User>> GetUsersAsync();
        Task<UserProfile> GetProfileAsync(int userId);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);
    }
}
