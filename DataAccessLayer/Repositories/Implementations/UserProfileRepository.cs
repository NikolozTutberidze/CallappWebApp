using DataAccessLayer.Data;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Abstracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.Implementations
{
    public class UserProfileRepository : IUserProfileRepository
    {
        private readonly CallappContext _context;

        public UserProfileRepository(CallappContext context) => _context = context;

        public async Task AddProfileAsync(UserProfile profile)
        {
            _context.UserProfiles.Add(profile);
            await _context.SaveChangesAsync();
        }

        public async Task<UserProfile> GetProfileByIdAsync(int id)
        {
            var profile = await _context.UserProfiles.FirstOrDefaultAsync(p => p.Id == id);

            return profile;
        }

        public async Task<ICollection<UserProfile>> GetProfilesAsync()
        {
            var profiles = await _context.UserProfiles.ToListAsync();

            return profiles;
        }

        public async Task UpdateProfileAsync(UserProfile profile)
        {
            _context.UserProfiles.Update(profile);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProfileAsync(int id)
        {
            var profile = await GetProfileByIdAsync(id);

            _context.UserProfiles.Remove(profile);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UserExistsAsync(int userid)
        {
            if (await _context.Users.AnyAsync(u => u.Id == userid))
                return true;
            else
                return false;
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            return user;
        }

        public void UpdateUser(User user)
        {
            _context.Users.Update(user);
        }
    }
}
