using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Database;
using WebApi.JWT;

namespace WebApi.Controllers;

[AllowAnonymous]
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly JWTService _jWTService;
    private readonly DataBase _database;

    public AuthController(JWTService jWTService, DataBase database)
    {
        _jWTService = jWTService;
        _database = database;
    }

    [HttpGet("Login")]
    public ActionResult<string> Login(string userName, string password)
    {
        var user = _database.GetByUserName(userName);
        if (user is not null)
        {
            var token = _jWTService.Generate(userName, password);
            return Ok(token);
        }
        return Unauthorized();

    }
}
