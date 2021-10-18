using API.Errors;
using API.Extensions;
using API.Models.Request;
using API.Models.Response;
using AutoMapper;
using Core.Entities.Identitiy;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class AccountController : MainController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService, IMapper mapper)
        {
            _userManager = userManager ?? throw new ArgumentException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentException(nameof(signInManager));
            _tokenService = tokenService ?? throw new ArgumentException(nameof(tokenService));
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserResponse>> GetCurrentUser()
        {
            var user = await _userManager.FindByEmailFromClaimsPrincipalAsync(User);

            return new UserResponse(user.Email, user.DisplayName, _tokenService.CreateToken(user));
        }

        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExists([FromQuery] string email)
        {
            return await _userManager.FindByEmailAsync(email) is not null;
        }

        [Authorize]
        [HttpGet("address")]
        public async Task<ActionResult<AddressResponse>> GetUserAddress()
        {
            var user = await _userManager.FindByClaimsPrincipalWithAddressAsync(User);

            return _mapper.Map<AddressResponse>(user.Address);
        }

        [Authorize]
        [HttpPut("address")]
        public async Task<ActionResult<AddressResponse>> UpdateUserAddress(AddressResponse address)
        {
            var user = await _userManager.FindByClaimsPrincipalWithAddressAsync(User);

            user.Address = _mapper.Map<Address>(address);

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded) return Ok(_mapper.Map<AddressResponse>(user.Address));

            return BadRequest("Problem updating the user");
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserResponse>> Login(LoginRequest loginRequest)
        {
            var user = await _userManager.FindByEmailAsync(loginRequest.Email);

            if (user is null) return Unauthorized(new ApiResponse((int)HttpStatusCode.Unauthorized));

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginRequest.Password, false);

            if (result.Succeeded) return new UserResponse(user.Email, user.DisplayName, _tokenService.CreateToken(user));

            return Unauthorized(new ApiResponse((int)HttpStatusCode.Unauthorized));            
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserResponse>> Register(RegisterRequest registerRequest)
        {
            if (CheckEmailExists(registerRequest.Email).Result.Value)
                return BadRequest(new ApiValidationErrorResponse(new[] { "Email address already in use" }));
            
            var user = new AppUser
            {
                DisplayName = registerRequest.DisplayName,
                Email = registerRequest.Email,
                UserName = registerRequest.Email
            };

            var result = await _userManager.CreateAsync(user, registerRequest.Password);

            if (result.Succeeded) return new UserResponse(user.Email, user.DisplayName, _tokenService.CreateToken(user));

            return BadRequest(new ApiResponse((int)HttpStatusCode.BadRequest));
        }
    }
}