using API.Errors;
using API.Models.Request;
using API.Models.Response;
using Core.Entities.Identitiy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class AccountController : MainController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager ?? throw new ArgumentException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentException(nameof(signInManager));
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserResponse>> Login(LoginRequest loginRequest)
        {
            var user = await _userManager.FindByEmailAsync(loginRequest.Email);

            if (user is null) return Unauthorized(new ApiResponse((int)HttpStatusCode.Unauthorized));

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginRequest.Password, false);

            if (!result.Succeeded) return Unauthorized(new ApiResponse((int)HttpStatusCode.Unauthorized));  

            return new UserResponse(user.Email, user.DisplayName, "This will be a token");
        }
    }
}