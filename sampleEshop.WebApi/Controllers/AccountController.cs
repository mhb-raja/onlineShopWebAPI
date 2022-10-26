using Core.DTOs.Account;
using Core.Services.Interfaces;
using Core.Utilities.Extensions;
using DataLayer.Entities.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using sampleEshop.WebApi.Utilities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace sampleEshop.WebApi.Controllers
{
    public class AccountController : SiteBaseController
    {
        #region constructor
        private readonly IUserService userService;
        public AccountController(IUserService userService)
        {
            this.userService = userService;
        }
        #endregion

        #region Register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDTO register)
        {
            if (ModelState.IsValid)
            {
                var res = await userService.RegisterUser(register);
                switch (res)
                {
                    case RegisterUserResult.EmailExists:
                        return JsonResponseStatus.Error("این ایمیل قبلا ثبت شده است");
                }
            }
            return JsonResponseStatus.Success();
        }
        #endregion

        #region Login

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO login)
        {
            if (ModelState.IsValid)
            {
                LoginUserResult res = await userService.LoginUser(login);
                switch (res)
                {
                    case LoginUserResult.IncorrectData:
                        return JsonResponseStatus.NotFound("کاربری با مشخصات وارد شده پیدا نشد");
                    case LoginUserResult.NotActivated:
                        return JsonResponseStatus.Error("حساب کاربری شما فعال نشده است");

                    case LoginUserResult.Success:
                        var user = await userService.GetUserByEmail(login.Email);
                        return JsonResponseStatus.Success(createLoggedInUser(user));
                }
            }

            return JsonResponseStatus.Error();
        }

        string createToken(User user)
        {
            //var user = await userService.GetUserByEmail(login.Email);
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Security.secretKeyStr));//"ProjectJwtBearer"
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokenOptions = new JwtSecurityToken(
                issuer: Security.issuer,
                claims: new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                },
                expires: DateTime.Now.AddDays(30),
                signingCredentials: signinCredentials
                );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return tokenString;
        }

        loggedInUserDTO createLoggedInUser(User user)
        {
            return new loggedInUserDTO
            {
                Token = createToken(user),// tokenString,
                ExpireTime = 30,
                Firstname = user.Firstname,//could be dto. user has classified info
                Lastname = user.Lastname,
                Id = user.Id,
                Address = user.Address,
                Mobile = user.Mobile
            };
        }
        #endregion

        #region Check user authentication
        [HttpPost("check-auth")]
        public async Task<IActionResult> CheckUserAuth()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await userService.GetUserById(User.GetUserId());
                if (user != null)
                    return JsonResponseStatus.Success(user);
            }
            return JsonResponseStatus.UnAuthorized();
        }

        #endregion

        #region Activate User Account

        [HttpGet("activate-account/{id}")]
        public async Task<IActionResult> ActivateAccount(string id)
        {
            var user = await userService.GetUserByEmailActiveCode(id);
            if (user != null)
            {
                userService.ActivateUser(user);
                return JsonResponseStatus.Success();
            }
            return JsonResponseStatus.NotFound();
        }
        #endregion

        #region edit user account
        [HttpPost("edit-user")]
        public async Task<IActionResult> EditUser([FromBody] UserDTO editUser)
        {
            if (User.Identity.IsAuthenticated)
            {
                long id = User.GetUserId();
                await userService.EditUserInfo(editUser,id);
                return JsonResponseStatus.Success(await userService.GetUserById(id));// message = "اطلاعات کاربر با موفقیت ویرایش شد" );
            }
            return JsonResponseStatus.UnAuthorized();
        }
        #endregion

        #region Sign Out
        [HttpGet("sign-out")]
        public async Task<IActionResult> LogOut()
        {
            if (User.Identity.IsAuthenticated)
            {
                await HttpContext.SignOutAsync();
                return JsonResponseStatus.Success();
            }
            return JsonResponseStatus.Error();
        }

        #endregion

        [HttpGet("test")]
        public async Task<IActionResult> test()
        {
            var details = await userService.GetAllUsers(); //orderService.GetUserShoppingCart(User.GetUserId());
            return JsonResponseStatus.Error("sayidiiiiii maro000");
        }
    }
}
