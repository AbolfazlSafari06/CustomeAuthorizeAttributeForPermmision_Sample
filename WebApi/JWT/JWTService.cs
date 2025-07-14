using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi.Database;

namespace WebApi.JWT;

public class JWTService
{
    public static readonly string jwtSalt = "291E89F8-778A-4486-8796-7B54775AEE83";
    public static readonly string jwtIssuer = "yourdomain.com";
    public static readonly string jwtAudience = "yourdomain.com";
    public readonly DataBase _database;

    public JWTService()
    {
        _database = new DataBase();
    }

    public string Generate(string userName, string password)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSalt));
        var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
        var user = _database.GetByUserName(userName);

        var claims = new List<Claim>
        {
            // Should We Add id To Claims
            new Claim("USERID", user.Id.ToString()),
            new Claim(ClaimTypes.Name, userName),
            new Claim(ClaimTypes.Role, "User"),
        };

        // Should We Add Permissions To Claims
         
        var tokenOptions = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: jwtAudience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(5),
            signingCredentials: signinCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }
}
