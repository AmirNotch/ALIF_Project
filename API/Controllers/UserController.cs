using System.Security.Claims;
using System.Threading.Tasks;
using API.Decryption;
using API.DTOs;
using API.Services;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly TokenService _tokenService;
        
        public UserController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, TokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }
        
        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDto)
        {
            var inn = long.Parse(DecryptClass.Decrypt(loginDto.INN));
            var password = DecryptClass.Decrypt(loginDto.Password);
            
            
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.INN == inn);

            if (user == null) return Unauthorized();

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);

            if (result.Succeeded)
            {
                return CreateUserObject(user);
            }
            
            return Unauthorized();
        }
        
        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDto)
        {
            var inn = long.Parse(DecryptClass.Decrypt(registerDto.INN));
            var email = DecryptClass.Decrypt(registerDto.Email);
            var surname = DecryptClass.Decrypt(registerDto.Surname);
            var name = DecryptClass.Decrypt(registerDto.Name);
            
            if (await _userManager.Users.AnyAsync(x => x.INN == inn && x.Email == email))
            {
                ModelState.AddModelError("email", "Email taken and INN taken");
                return ValidationProblem();
            }
            
            var user = new AppUser
            {
                INN = inn,
                Surname = surname,
                UserName = name,
                Name = name
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            
            if (result.Succeeded)
            {
                return CreateUserObject(user);
            }

            return BadRequest("Problem registering user");
        }
        
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(x => x.Email == User.FindFirstValue(ClaimTypes.Email));

            return CreateUserObject(user);
        }
        
        private UserDTO CreateUserObject(AppUser user)
        {
            return new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Token = _tokenService.CreateToken(user),
            };
        }
    }
}