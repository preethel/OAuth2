using Google.Apis.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProjectX.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;

    public AuthController(IConfiguration config)
    {
        _config = config;
    }

    [HttpPost("google-login")]
    public async Task<IActionResult> GoogleLogin([FromBody] TokenDto body)
    {
        var validPayload = await GoogleJsonWebSignature.ValidateAsync(body.IdToken);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, validPayload.Subject),
            new Claim(ClaimTypes.Name, validPayload.Name),
            new Claim(ClaimTypes.Email, validPayload.Email),
            new Claim("picture", validPayload.Picture),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your_secret_keydsfsdfasdfasdfasdfasdfa_here"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "your_api_url",
            audience: "your_client_url",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds);

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return Ok(new { token = tokenString });
    }
    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        return Ok(new { user = User.Identity.Name });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return Ok();
    }
}

public class TokenDto
{
    public string IdToken { get; set; }
}
