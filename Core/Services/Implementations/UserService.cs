using Core.DTOs.Account;
using Core.Security;
using Core.Services.Interfaces;
using DataLayer.Entities.Access;
using DataLayer.Entities.Account;
using DataLayer.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Implementations
{
    public class UserService : IUserService
    {
        #region constructor
        private readonly IGenericRepository<User> userRepository;
        private readonly IPasswordHelper passwordHelper;
        //private readonly IMailSender mailSender;
        //private readonly IViewRenderService renderView;
        private readonly IGenericRepository<UserRole> userRoleRepository;
        public UserService(IGenericRepository<User> userRepository, IPasswordHelper passwordHelper,
             //IMailSender mailSender, IViewRenderService renderView,
             IGenericRepository<UserRole> userRoleRepository
            )
        {
            this.userRepository = userRepository;
            this.passwordHelper = passwordHelper;
            //this.mailSender = mailSender;
            //this.renderView = renderView;
            this.userRoleRepository = userRoleRepository;
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            userRepository?.Dispose();
        }

        #endregion

        #region User Section
        public async Task<List<User>> GetAllUsers()
        {
            return await userRepository.GetEntitiesQuery().ToListAsync();
        }

        public async Task<RegisterUserResult> RegisterUser(UserDTO register)
        {
            if (UserExistsByEmail(register.Email))
                return RegisterUserResult.EmailExists;

            var user = new User
            {
                Email = register.Email.SanitizeText(),
                Address = register.Address.SanitizeText(),
                Firstname = register.Firstname.SanitizeText(),
                Lastname = register.Lastname.SanitizeText(),
                Password = passwordHelper.EncodePasswordMd5(register.Password),
                EmailActiveCode = Guid.NewGuid().ToString(),
                Mobile = register.Mobile.SanitizeText(),
                IsActivated = false
            };

            await userRepository.AddEntity(user);
            await userRepository.SaveChanges();

            //var body = await renderView.RenderToStringAsync("Email/ActivateAccount", user);
            //mailSender.Send(user.Email, "تست فعال سازی", body);  ????????????????????

            return RegisterUserResult.Success;
        }

        public bool UserExistsByEmail(string email)
        {
            return userRepository.GetEntitiesQuery().Any(s => s.Email == email.ToLower().Trim());
        }

        public async Task<LoginUserResult> LoginUser(LoginDTO login, bool checkAdminRole = false)
        {
            var password = passwordHelper.EncodePasswordMd5(login.Password);
            var user = await userRepository.GetEntitiesQuery().
                SingleOrDefaultAsync(s => s.Email == login.Email.ToLower().Trim() && s.Password == password);
            if (user == null)
                return LoginUserResult.IncorrectData;
            if (!user.IsActivated)
                return LoginUserResult.NotActivated;
            if (checkAdminRole)
            {
                if (!await IsUserAdmin(user.Id))
                    return LoginUserResult.NotAdmin;
            }
            return LoginUserResult.Success;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await userRepository.GetEntitiesQuery().SingleOrDefaultAsync(s => s.Email == email.ToLower().Trim());
        }

        public async Task<UserDTO> GetUserById(long userId)
        {
            var user = await userRepository.GetEntityById(userId);
            if (user == null) return null;
            return new UserDTO
            {
                Id = user.Id,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Address = user.Address,
                Email = user.Email,
                Mobile = user.Mobile,
                Password = ""
            };
        }

        public void ActivateUser(User user)
        {
            user.IsActivated = true;
            user.EmailActiveCode = Guid.NewGuid().ToString();
            userRepository.UpdateEntity(user);
            userRepository.SaveChanges();
        }

        public Task<User> GetUserByEmailActiveCode(string emailActiveCode)
        {
            return userRepository.GetEntitiesQuery().SingleOrDefaultAsync(s => s.EmailActiveCode == emailActiveCode);
        }

        public async Task EditUserInfo(UserDTO userInfo, long userId)
        {
            var user = await userRepository.GetEntityById(userId); //await GetUserById(userId);
            if (user != null)
            {
                user.Firstname = userInfo.Firstname;
                user.Lastname = userInfo.Lastname;
                user.Address = userInfo.Address;
                user.Email = userInfo.Email;
                user.Mobile = userInfo.Mobile;
                user.Password = userInfo.Password;
                userRepository.UpdateEntity(user);
                await userRepository.SaveChanges();
            }
        }

        public async Task<bool> IsUserAdmin(long userId)
        {
            return await userRoleRepository.GetEntitiesQuery()
                    .Include(s => s.Role)
                    .AnyAsync(s => s.UserId == userId && s.Role.Name == "Admin");
        }
        #endregion
    }
}
