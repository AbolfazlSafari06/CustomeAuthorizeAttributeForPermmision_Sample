using Microsoft.AspNetCore.Mvc;
using WebApi.Attributes;
using WebApi.Database;
using WebApi.Models;


namespace WebApi.Controllers;

[CustomAuthorize]
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly DataBase _database;

    public UserController(DataBase database)
    {
        _database = database;
    }

    [HttpGet]
    [CustomAuthorize("string1")]
    public ActionResult<List<User>> Get()
    {
        return _database.GetList();
    } 
     
    [HttpGet("{id}")]
    [CustomAuthorize("string2")]
    public ActionResult<User> Get(int id)
    {
        return _database.Get(id);
    }

    [HttpPost]
    [CustomAuthorize("string3")]
    public ActionResult Post([FromBody] User value)
    {
        _database.Create(value);
        return Ok();
    }

    [HttpPut("{id}")]
    public ActionResult Put(int id, [FromBody] User value)
    {
        var isSuccesfull = _database.Update(id, value);
        if (isSuccesfull)
        {
            return Ok();
        }
        else
        {
            return BadRequest();
        }
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        var isSuccesfull = _database.Delete(id);
        if (isSuccesfull)
        {
            return Ok();
        }
        else
        {
            return BadRequest();
        }
    }
}
