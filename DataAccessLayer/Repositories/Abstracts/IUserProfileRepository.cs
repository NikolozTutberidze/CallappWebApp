using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.Abstracts
{
    public interface IUserProfileRepository
    {
        Task AddProfileAsync(UserProfile profile);
        Task<UserProfile> GetProfileByIdAsync(int id);
        Task<ICollection<UserProfile>> GetProfilesAsync();
        Task UpdateProfileAsync(UserProfile profile);
        Task DeleteProfileAsync(int id);
        Task<bool> UserExistsAsync(int userid);
        Task<User> GetUserByIdAsync(int userId);
        void UpdateUser(User user);
    }
}
