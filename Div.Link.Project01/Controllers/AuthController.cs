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

        // Assign default role
        await _userManager.AddToRoleAsync(user, "User");

        var accessToken = await _servicesAuth.GenerateToken(user);
        var refreshToken = _servicesAuth.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
        await _userManager.UpdateAsync(user);

        return Ok(new TokenDTO
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        });
    }

    // Login (Email + Password)
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            return Unauthorized("Invalid credentials");

        var accessToken = await _servicesAuth.GenerateToken(user);
        var refreshToken = _servicesAuth.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
        await _userManager.UpdateAsync(user);

        return Ok(new TokenDTO
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        });
    }

    // Refresh Token
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] TokenDTO tokenDto)
    {
        if (tokenDto is null)
            return BadRequest("Invalid client request");

        var principal = _servicesAuth.GetPrincipalFromExpiredToken(tokenDto.AccessToken);
        var username = principal.Identity.Name;
        var user = await _userManager.FindByNameAsync(username);

        if (user == null || user.RefreshToken != tokenDto.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            return BadRequest("Invalid client request");

        var newAccessToken = await _servicesAuth.GenerateToken(user);
        var newRefreshToken = _servicesAuth.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        await _userManager.UpdateAsync(user);

        return Ok(new TokenDTO
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        });
    }

    //  Google Login
    [HttpGet("google-login")]
    public IActionResult GoogleLogin()
    {
        var properties = new AuthenticationProperties { RedirectUri = Url.Action("GoogleCallback") };
        return Challenge(properties, GoogleDefaults.AuthenticationScheme);
    }

    [HttpGet("google-callback")]
    public async Task<IActionResult> GoogleCallback()
    {
        var result = await HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);
        if (!result.Succeeded) return Unauthorized();

        var email = result.Principal.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
        {
            user = new ApplicationUser { UserName = email, Email = email };
            await _userManager.CreateAsync(user);
            await _userManager.AddToRoleAsync(user, "User");
        }

        var accessToken = await _servicesAuth.GenerateToken(user);
        var refreshToken = _servicesAuth.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
        await _userManager.UpdateAsync(user);

        // Redirect back to dashboard with the token
        return Redirect($"/index.html?token={accessToken}");
    }
}