using Microsoft.AspNetCore.Identity;
using PartyProduct_Exercise_03.Models;
using PartyProduct_Exercise_03.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PartyProduct_Exercise_03.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUserService _userService;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;

        public AccountRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IUserService userService, IPasswordHasher<ApplicationUser> passwordHasher)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userService = userService;
            _passwordHasher = passwordHasher;
        }

        public async Task<IdentityResult> CreateUserAsync(SignUpUserModel userModel)
        {
            var user = new ApplicationUser()
            {
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                Email = userModel.Email,
                UserName = userModel.Email,
            };

            var result = await _userManager.CreateAsync(user);
            var userId = user.Id;
            var Getuser = await _userManager.FindByIdAsync(userId);

            MD5 md5 = new MD5CryptoServiceProvider();
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(userModel.Password));
            byte[] hashedPass = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();

            for (int i = 0; i < hashedPass.Length; i++)
            {
                strBuilder.Append(hashedPass[i].ToString("x2"));
            }
            await _userManager.AddPasswordAsync(Getuser, strBuilder.ToString());
            return result;
        }

        public async Task<SignInResult> LoginAsync(SignInModel signInModel)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(signInModel.Password));
            byte[] hashedPass = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();

            for (int i = 0; i < hashedPass.Length; i++)
            {
                strBuilder.Append(hashedPass[i].ToString("x2"));
            }

            var result = await _signInManager.PasswordSignInAsync(signInModel.Email, strBuilder.ToString(), signInModel.RememberMe, false);
            return result;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> ChangeYourPassword(ChangePasswordModel model)
        {
            var userId = _userService.GetUserId();
            var user = await _userManager.FindByIdAsync(userId);

            MD5 md5Cur = new MD5CryptoServiceProvider();
            md5Cur.ComputeHash(ASCIIEncoding.ASCII.GetBytes(model.CurrentPassword));
            byte[] hashedCurPass = md5Cur.Hash;

            MD5 md5New = new MD5CryptoServiceProvider();
            md5New.ComputeHash(ASCIIEncoding.ASCII.GetBytes(model.NewPassword));
            byte[] hashedNewPass = md5New.Hash;

            StringBuilder strCurBuilder = new StringBuilder();
            StringBuilder strNewBuilder = new StringBuilder();

            for (int i = 0; i < hashedCurPass.Length; i++)
            {
                strCurBuilder.Append(hashedCurPass[i].ToString("x2"));
            }

            for (int i = 0; i < hashedNewPass.Length; i++)
            {
                strNewBuilder.Append(hashedNewPass[i].ToString("x2"));
            }
            return await _userManager.ChangePasswordAsync(user, strCurBuilder.ToString(), strNewBuilder.ToString());
        }
    }
}
