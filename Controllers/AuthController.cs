using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;

    public AuthController(IConfiguration config)
    {
        _config = config;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest login)
    {
        // Aqui você faria a validação do usuário no banco e buscaria os papéis (roles)
        if (IsValidUser(login, out List<string> roles))
        {
            var token = GenerateToken(login.Username, roles);
            return Ok(new { token });
        }

        return Unauthorized();
    }

    private bool IsValidUser(LoginRequest login, out List<string> roles)
    {
        roles = new List<string>();

        // Exemplo estático, substituir por validação real no banco
        if (login.Username == "admin" && login.Password == "admin123")
        {
            roles.Add("ADMIN");
            return true;
        }
        else if (login.Username == "nutri" && login.Password == "nutri123")
        {
            roles.Add("NUTRITIONIST");
            return true;
        }

        return false;
    }

    private string GenerateToken(string username, List<string> roles)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, username),
        };

        foreach (var role in roles)
            claims.Add(new Claim(ClaimTypes.Role, role));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(2),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

public class LoginRequest
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
}
