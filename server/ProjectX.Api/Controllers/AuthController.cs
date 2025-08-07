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

    [Authorize(AuthenticationSchemes = "Google")]
    [HttpPost("google-login")]
    public async Task<IActionResult> GoogleLogin()
    {
        // 1. Token read from Authorization header
        var authHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
        {
            return Unauthorized(new { error = "Missing or invalid Authorization header" });
        }

        var idToken = authHeader.Substring("Bearer ".Length).Trim();

        // 2. Validate Google ID Token
        GoogleJsonWebSignature.Payload validPayload;
        try
        {
            validPayload = await GoogleJsonWebSignature.ValidateAsync(idToken);
        }
        catch (InvalidJwtException ex)
        {
            return Unauthorized(new { error = "Invalid Google token", details = ex.Message });
        }

        // 3. Issue your own JWT token
        var claims = new[]
        {
        new Claim(ClaimTypes.NameIdentifier, validPayload.Subject),
        new Claim(ClaimTypes.Name, validPayload.Name),
        new Claim(ClaimTypes.Email, validPayload.Email),
        new Claim("picture", validPayload.Picture ?? "")
    };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your_secret_keydsfsdfasdfasdfasdfasdfa_here"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var jwt = new JwtSecurityToken(
            issuer: "your_api_url",
            audience: "your_client_url",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds);

        var tokenString = new JwtSecurityTokenHandler().WriteToken(jwt);

        return Ok(new { token = tokenString });
    }

    [Authorize(AuthenticationSchemes = "Internal")]
    [HttpGet("me")]
    public IActionResult Me()
    {
        var name = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        return Ok(new { user = name });
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
