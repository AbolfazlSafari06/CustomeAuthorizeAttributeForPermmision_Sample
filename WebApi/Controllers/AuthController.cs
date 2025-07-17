using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.JWT;
using WebApi.Repository;

namespace WebApi.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly JWTService _jWTService;
    private readonly IUserRepository _userRepository;

    public AuthController(JWTService jWTService, IUserRepository userRepository)
    {
        _jWTService = jWTService;
        _userRepository = userRepository;
    }

    [HttpGet("Login")]
    public ActionResult<string> Login(string userName, string password)
    {
        var user = _userRepository.GetByUserName(userName);
        if (user is not null)
        {
            var token = _jWTService.Generate(userName, password);
            return Ok(token);
        }
        return Unauthorized();

    }
}
