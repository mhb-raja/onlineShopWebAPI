using Core.DTOs.Account;
using DataLayer.Entities.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Interfaces
{
    public interface IUserService : IDisposable
    {
        Task<List<User>> GetAllUsers();

        Task<RegisterUserResult> RegisterUser(UserDTO register);
        bool UserExistsByEmail(string email);

        Task<LoginUserResult> LoginUser(LoginDTO login, bool checkAdminRole = false);
        Task<User> GetUserByEmail(string email);
        Task<UserDTO> GetUserById(long userId);


        void ActivateUser(User user);
        Task<User> GetUserByEmailActiveCode(string emailActiveCode);
        Task EditUserInfo(UserDTO user, long userId);
        Task<bool> IsUserAdmin(long userId);
        
    }
}
