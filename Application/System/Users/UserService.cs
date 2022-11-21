using DaisyStudy.Data;
using DaisyStudy.Models.Common;
using DaisyStudy.Models.System.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DaisyStudy.Application.System.Users
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        public UserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }
        public async Task<bool> Delete(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return false;
            }
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
                return true;

            return false;
        }

        public async Task<UserViewModel> GetById(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return null;
            }
            var roles = await _userManager.GetRolesAsync(user);
            var userViewModel = new UserViewModel()
            {
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                FirstName = user.FirstName,
                Dob = user.Dob,
                Id = user.Id,
                LastName = user.LastName,
                UserName = user.UserName,
                Roles = roles
            };
            return userViewModel;
        }

        public async Task<UserViewModel> GetByName(string UserName)
        {
            var user = await _userManager.FindByNameAsync(UserName);
            if (user == null)
            {
                return null;
            }
            var roles = await _userManager.GetRolesAsync(user);
            var userViewModel = new UserViewModel()
            {
                Email = user.Email,
                Avatar = user.Avatar,
                PhoneNumber = user.PhoneNumber,
                FirstName = user.FirstName,
                FullName = user.FirstName + " " + user.LastName,
                Dob = user.Dob,
                Id = user.Id,
                LastName = user.LastName,
                UserName = user.UserName,
                Roles = roles
            };
            return userViewModel;
        }

        public async Task<UserViewModel> GetByEmail(string Email)
        {
            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null)
            {
                return null;
            }
            var roles = await _userManager.GetRolesAsync(user);
            var userViewModel = new UserViewModel()
            {
                Email = user.Email,
                Avatar = user.Avatar,
                PhoneNumber = user.PhoneNumber,
                FirstName = user.FirstName,
                FullName = user.FirstName + " " + user.LastName,
                Dob = user.Dob,
                Id = user.Id,
                LastName = user.LastName,
                UserName = user.UserName,
                Roles = roles
            };
            return userViewModel;
        }

        public async Task<PagedResult<UserViewModel>> GetUsersPaging(GetUserPagingRequest request)
        {
            var query = _userManager.Users;
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.UserName.Contains(request.Keyword)
                || x.PhoneNumber.Contains(request.Keyword)
                || x.Email.Contains(request.Keyword)
                || x.FirstName.Contains(request.Keyword)
                || x.LastName.Contains(request.Keyword));
            }

            //3. Paging
            int totalRow = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new UserViewModel()
                {
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    UserName = x.UserName,
                    FirstName = x.FirstName,
                    Id = x.Id,
                    Dob = x.Dob,
                    LastName = x.LastName
                }).ToListAsync();

            //4. Select and projection
            var pagedResult = new PagedResult<UserViewModel>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };
            return pagedResult;
        }

        public async Task<bool> DepositMoney(string UserName, decimal money)
        {
            var user = await _userManager.FindByNameAsync(UserName);
            if (user is null) return false;
            user.AccountBalance += money;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return true;
            }
            return false;
        }
    }
}