using Microsoft.AspNetCore.Identity;
using PartyProduct_Exercise_03.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PartyProduct_Exercise_03.Repository
{
    public interface IAccountRepository
    {
        Task<IdentityResult> CreateUserAsync(SignUpUserModel userModel);
        Task<SignInResult> LoginAsync(SignInModel signInModel);
        Task LogoutAsync();
        Task<IdentityResult> ChangeYourPassword(ChangePasswordModel model);
    }
}
