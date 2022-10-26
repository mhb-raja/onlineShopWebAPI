using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Account
{
    public class LoginDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class UserDTO : LoginDTO
    {
        public long Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        //public string EmailActiveCode { get; set; }
        //public bool IsActivated { get; set; }
    }
    public class loggedInUserDTO : UserDTO
    {
        public string Token { get; set; }
        public int ExpireTime { get; set; }
        //firstname = user.Firstname,//could be dto. user has classified info
        //lastname = user.Lastname,
        //userId = user.Id,
        //address = user.Address,
        //mobile = user.Mobile
    }
    public enum RegisterUserResult
    {
        Success,
        EmailExists
    }

    public enum LoginUserResult
    {
        Success,
        IncorrectData,
        NotActivated,
        NotAdmin
    }
}
