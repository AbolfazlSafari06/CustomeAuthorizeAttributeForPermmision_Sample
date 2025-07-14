using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using WebApi.Database;
using WebApi.JWT;

namespace WebApi.Middlewares;

public class CustomAuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<CustomAuthenticationMiddleware> _logger;
    private readonly DataBase _database;
    private readonly JWTService _jwtSettings;

    public CustomAuthenticationMiddleware(
        RequestDelegate next,
        ILogger<CustomAuthenticationMiddleware> logger,
        DataBase database,
        JWTService jwtSettings)
    {
        _next = next;
        _logger = logger;
        _database = database;
        _jwtSettings = jwtSettings;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token != null)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(JWTService.jwtSalt);

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidIssuer = JWTService.jwtIssuer,
                    ValidAudience = JWTService.jwtAudience,
                }, out _);
            }
            catch
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Invalid token");
                return;
            }
        }

        await _next(context);
    }
}
