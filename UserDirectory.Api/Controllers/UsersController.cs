using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserDirectory.Api.Data;
using UserDirectory.Api.DTOs;
using UserDirectory.Api.Models;

namespace UserDirectory.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(AppDbContext context) : ControllerBase
{
    // GET: api/users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        var users = await context.Users
            .AsNoTracking()
            .OrderBy(user => user.Id)
            .ToListAsync();

        return Ok(users);
    }

    // GET: api/users/1
    [HttpGet("{id:int}")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        var user = await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Id == id);

        return user is null
            ? NotFound(new { message = $"User with ID {id} was not found." })
            : Ok(user);
    }

    // POST: api/users
    [HttpPost]
    public async Task<ActionResult<User>> CreateUser(
        [FromBody] CreateUserDto request)
    {
        var user = new User
        {
            Name = request.Name.Trim(),
            Age = request.Age,
            City = request.City.Trim(),
            State = request.State.Trim(),
            Pincode = request.Pincode.Trim()
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
    }

    // PUT: api/users/1
    [HttpPut("{id:int}")]
    public async Task<ActionResult<User>> UpdateUser(
        int id, [FromBody] UpdateUserDto request)
    {
        var user = await context.Users.FindAsync(id);

        if (user is null)
            return NotFound(new { message = $"User with ID {id} was not found." });

        user.Name = request.Name.Trim();
        user.Age = request.Age;
        user.City = request.City.Trim();
        user.State = request.State.Trim();
        user.Pincode = request.Pincode.Trim();

        await context.SaveChangesAsync();
        return Ok(user);
    }

    // DELETE: api/users/1
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await context.Users.FindAsync(id);

        if (user is null)
            return NotFound(new { message = $"User with ID {id} was not found." });

        context.Users.Remove(user);
        await context.SaveChangesAsync();

        return NoContent();
    }
}
