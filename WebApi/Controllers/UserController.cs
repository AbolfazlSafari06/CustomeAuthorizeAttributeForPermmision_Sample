using Microsoft.AspNetCore.Mvc;
using WebApi.Attributes;
using WebApi.Database;
using WebApi.Models;
using WebApi.Repository;


namespace WebApi.Controllers;

[CustomAuthorize]
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpGet]
    [CustomAuthorize("strign1")]
    public ActionResult<List<User>> Get()
    {
        return _userRepository.GetList();
    } 
     
    [HttpGet("{id}")]
    [CustomAuthorize("strign2")]
    public ActionResult<User> Get(int id)
    {
        return _userRepository.Get(id);
    }

    [HttpPost]
    [CustomAuthorize("strign3")]
    public ActionResult Post([FromBody] User value)
    {
        return Ok();
        //_userRepository.Create(value);
        //return Ok();
    }

    [HttpPut("{id}")]
    [CustomAuthorize("strign4")]
    public ActionResult Put(int id, [FromBody] User value)
    {
        return Ok();
        //var isSuccesfull = _userRepository.Update(id, value);
        //if (isSuccesfull)
        //{
        //    return Ok();
        //}
        //else
        //{
        //    return BadRequest();
        //}
    }

    [HttpDelete("{id}")]
    [CustomAuthorize("strign5")]
    public ActionResult Delete(int id)
    {
        return Ok();
        //var isSuccesfull = _userRepository.Delete(id);
        //if (isSuccesfull)
        //{
        //    return Ok();
        //}
        //else
        //{
        //    return BadRequest();
        //}
    }
}
