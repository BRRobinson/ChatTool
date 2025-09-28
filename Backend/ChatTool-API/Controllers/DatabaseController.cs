using ChatTool.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatTool.API.Controllers;

[ApiController]
[Route("[controller]")]
public class DatabaseController : ControllerBase
{
    private readonly DBContext _dbContext;
    public DatabaseController(DBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public DBContext DbContext { get; }

    // GET /recipes
    [HttpGet("DBSync")]
    public IActionResult DBSync()
    {
        bool canConnect = _dbContext.Database.CanConnect();
        _dbContext.Database.Migrate();
        return Ok();
    }
}
