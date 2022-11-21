﻿using DaisyStudy.Core.Repositories;
using DaisyStudy.Data;

namespace DaisyStudy.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public ICollection<ApplicationUser> GetUsers(string? keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                return _context.Users.Where(x=> x.FirstName.Contains(keyword)
                || x.LastName.Contains(keyword)
                || x.Email.Contains(keyword)
                || x.PhoneNumber.Contains(keyword)).OrderBy(x => x.LastName).ToList();
            }
            return _context.Users.OrderBy(x => x.LastName).ToList();
        }

        public async Task<ApplicationUser> GetUser(string id)
        {
            return await _context.Users.FindAsync(id);
        }

        public ApplicationUser UpdateUser(ApplicationUser user)
        {
            _context.Update(user);
            _context.SaveChanges();

            return user;
        }
    }
}
