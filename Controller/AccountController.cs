using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Glasses.Data;
using Glasses.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Glasses.Controller
{

    [Route("api/[controller]")]
    [ApiController]

   
    public class AccountController : ControllerBase
    {
       // private readonly UserManager<ApiUser> _userManager;
        //private readonly SignInManager<ApiUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly ILogger<AccountController> _logger;
        private IConfiguration _config;
        public readonly GlassesContext _context;

        public AccountController( 
            ILogger<AccountController> logger,IMapper mapper, IConfiguration config, GlassesContext context)
        {
            //_userManager = userManager;
            //_signInManager = signInManager; SignInManager<ApiUser> signInManager,
            _logger = logger;
            _mapper = mapper;
            _config = config;
            _context = context;
        }


        [HttpPost]
        public async Task<IActionResult> Login(ApiUser apiUser)
        {
            if (apiUser != null && apiUser.UserName != null && apiUser.Password != null)
            {
                //var userData = await GetUser(apiUser.UserName, apiUser.Password);

                    var jwt = _config.GetSection("Jwt").Get<Jwt>();
                    if (apiUser != null)
                    {
                        var claims = new[]
                        {
                        new  Claim(JwtRegisteredClaimNames.Sub,jwt.Subject),
                        new  Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new  Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new  Claim("Id",apiUser.UserId.ToString()),
                        new  Claim("UserName",apiUser.UserName),
                        new  Claim("Password",apiUser.Password)
                    };
                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.key));
                        var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(
                            jwt.Issuer,
                            jwt.Audience,
                            claims,
                            expires: DateTime.Now.AddDays(1),
                            signingCredentials: signIn
                            );
                        return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                    }
                    else
                    {
                        return BadRequest("Invalid credentials...");
                    }
                }
                else
                {
                    return BadRequest("Invalid credentials...");
                }
        }

        [HttpGet]
        public async Task<ApiUser> GetUser(string userName, string password)
        {
            return await _context.ApiUsers.FirstOrDefaultAsync(u=>u.UserName== userName && u.Password==password);
        }

        //[AllowAnonymous]
        //[HttpPost]
        //public IActionResult APILogin([FromBody] ApiUser login)
        //{
        //    IActionResult response = Unauthorized();
        //    var user = AuthenticateUser(login);

        //    if (user != null)
        //    {
        //        var tokenString = GenerateJSONWebToken(user);
        //        response = Ok(new { token = tokenString });
        //    }

        //    return response;
        //}

        //[HttpPost]
        //public string GenerateJSONWebToken(ApiUser userInfo)
        //{
        //    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        //    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        //    var token = new JwtSecurityToken(_config["Jwt:Issuer"],
        //      _config["Jwt:Issuer"],
        //      null,
        //      expires: DateTime.Now.AddMinutes(120),
        //      signingCredentials: credentials);

        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}

        //[HttpPost]
        //public ApiUser AuthenticateUser(ApiUser login)
        //{
        //    ApiUser user = null;

        //    //Validate the User Credentials    
        //    //Demo Purpose, I have Passed HardCoded User Information    
        //    if (login.UserName == "admin")
        //    {
        //        user = new ApiUser { UserName = "admin", Password = "admin" };
        //    }
        //    return user;
        //}

        //[HttpPost]
        //[Route("Register")]
        //public async Task<IActionResult> Register([FromBody] UserDTO userDTO)
        //{
        //    _logger.LogInformation($"Registration attempt for {userDTO.Email}");
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    try
        //    {
        //        var user = _mapper.Map<ApiUser>(userDTO);
        //        user.UserName = userDTO.Email;
        //        var result = await _userManager.CreateAsync(user);
        //        if (!result.Succeeded)
        //        {
        //            foreach (var error in result.Errors)
        //            {
        //                ModelState.AddModelError(error.Code, error.Description);
        //            }
        //            return BadRequest(ModelState);
        //        }

        //        return Accepted();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, $"Something went wrong in the {nameof(Register)}");
        //        return Problem($"Something Went Wrong in the {nameof(Register)}", statusCode: 500);
        //    }
        //}

        //[HttpPost]
        //[Route("login")]

        //public async Task<IActionResult> Login([FromBody] LoginUserDTO userDTO)
        //{
        //    _logger.LogInformation($"Login attempt for {userDTO.Email}");
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    try
        //    {

        //        var result = await _signInManager.PasswordSignInAsync(userDTO.Email, userDTO.Password, false, false);

        //        if (!result.Succeeded)
        //        {
        //            return Unauthorized(userDTO);

        //            //BadRequest("$User Login failed  ");
        //        }
        //        return Accepted();

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, $"Something went wrong in the {nameof(Login)}");
        //        return Problem($"Something Went Wrong in the {nameof(Login)}", statusCode: 500);
        //    }
        //}

    }
}

