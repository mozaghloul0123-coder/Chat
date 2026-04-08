using Div.Link.Project01.BLL.Dto.Auth;
using Div.Link.Project01.BLL.Service;
using Div.Link.Project01.DAL.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IServicesAuth _servicesAuth;

    public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IServicesAuth servicesAuth)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _servicesAuth = servicesAuth;
    }

    // Register
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDTO model)
    {
        var user = new ApplicationUser { UserName = model.UserName, Email = model.Email };
        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        var token = await _servicesAuth.GenerateToken(user);
        return Ok(new { Token = token });
    }

    // Login (Email + Password)
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            return Unauthorized("Invalid credentials");

        var token = await _servicesAuth.GenerateToken(user);
        return Ok(new { Token = token });
    }

    //  Google Login
    [HttpGet("google-login")]
    public IActionResult GoogleLogin()
    {
        var properties = new AuthenticationProperties { RedirectUri = "/api/auth/Login" };
        return Challenge(properties, GoogleDefaults.AuthenticationScheme);
    }

    [HttpGet("google-callback")]
    public async Task<IActionResult> GoogleCallback()
    {
        var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        if (!result.Succeeded) return Unauthorized();

        var email = result.Principal.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
        {
            // Create user if not exists
            user = new ApplicationUser
            {
                UserName = email,
                Email = email
            };
            await _userManager.CreateAsync(user);
        }

        var token = await _servicesAuth.GenerateToken(user);
        return Ok(new { Token = token });
    }
}