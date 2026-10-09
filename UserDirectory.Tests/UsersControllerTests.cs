using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserDirectory.Api.Controllers;
using UserDirectory.Api.Data;
using UserDirectory.Api.DTOs;
using UserDirectory.Api.Models;
using Xunit;

namespace UserDirectory.Tests;

public class UsersControllerTests
{
    private static (AppDbContext Context, UsersController Controller) CreateController()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new AppDbContext(options);
        return (context, new UsersController(context));
    }

    private static CreateUserDto ValidUser() => new()
    {
        Name = "Rahul Sharma",
        Age = 28,
        City = "Bengaluru",
        State = "Karnataka",
        Pincode = "560001"
    };

    [Fact]
    public async Task GetUsers_ReturnsAllUsers()
    {
        var (context, controller) = CreateController();
        context.Users.AddRange(
            new User { Name = "Rahul", Age = 28, City = "Bengaluru", State = "Karnataka", Pincode = "560001" },
            new User { Name = "Priya", Age = 25, City = "Mysuru", State = "Karnataka", Pincode = "570001" });
        await context.SaveChangesAsync();

        var result = await controller.GetUsers();
        var response = Assert.IsType<OkObjectResult>(result.Result);
        var users = Assert.IsAssignableFrom<IEnumerable<User>>(response.Value);

        Assert.Equal(2, users.Count());
        await context.DisposeAsync();
    }

    [Fact]
    public async Task GetUsers_WhenEmpty_ReturnsEmptyList()
    {
        var (context, controller) = CreateController();

        var result = await controller.GetUsers();
        var response = Assert.IsType<OkObjectResult>(result.Result);
        var users = Assert.IsAssignableFrom<IEnumerable<User>>(response.Value);

        Assert.Empty(users);
        await context.DisposeAsync();
    }

    [Fact]
    public async Task GetUser_WhenExists_ReturnsUser()
    {
        var (context, controller) = CreateController();
        var user = new User { Name = "Rahul", Age = 28, City = "Bengaluru", State = "Karnataka", Pincode = "560001" };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var result = await controller.GetUser(user.Id);
        var response = Assert.IsType<OkObjectResult>(result.Result);
        var returnedUser = Assert.IsType<User>(response.Value);

        Assert.Equal(user.Id, returnedUser.Id);
        Assert.Equal("Rahul", returnedUser.Name);
        await context.DisposeAsync();
    }

    [Fact]
    public async Task GetUser_WhenMissing_ReturnsNotFound()
    {
        var (context, controller) = CreateController();

        var result = await controller.GetUser(999);

        Assert.IsType<NotFoundObjectResult>(result.Result);
        await context.DisposeAsync();
    }

    [Fact]
    public async Task CreateUser_WithValidData_ReturnsCreatedUser()
    {
        var (context, controller) = CreateController();

        var result = await controller.CreateUser(ValidUser());
        var response = Assert.IsType<CreatedAtActionResult>(result.Result);
        var createdUser = Assert.IsType<User>(response.Value);

        Assert.True(createdUser.Id > 0);
        Assert.Equal("Rahul Sharma", createdUser.Name);
        Assert.Equal(1, await context.Users.CountAsync());
        await context.DisposeAsync();
    }

    [Fact]
    public async Task UpdateUser_WhenExists_UpdatesUser()
    {
        var (context, controller) = CreateController();
        var user = new User { Name = "Rahul", Age = 28, City = "Bengaluru", State = "Karnataka", Pincode = "560001" };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var request = new UpdateUserDto
        {
            Name = "Rahul Sharma",
            Age = 29,
            City = "Mysuru",
            State = "Karnataka",
            Pincode = "570001"
        };

        var result = await controller.UpdateUser(user.Id, request);
        var response = Assert.IsType<OkObjectResult>(result.Result);
        var updatedUser = Assert.IsType<User>(response.Value);

        Assert.Equal(29, updatedUser.Age);
        Assert.Equal("Mysuru", updatedUser.City);
        await context.DisposeAsync();
    }

    [Fact]
    public async Task UpdateUser_WhenMissing_ReturnsNotFound()
    {
        var (context, controller) = CreateController();

        var result = await controller.UpdateUser(999, new UpdateUserDto
        {
            Name = "Rahul Sharma",
            Age = 28,
            City = "Bengaluru",
            State = "Karnataka",
            Pincode = "560001"
        });

        Assert.IsType<NotFoundObjectResult>(result.Result);
        await context.DisposeAsync();
    }

    [Fact]
    public async Task DeleteUser_WhenExists_DeletesUser()
    {
        var (context, controller) = CreateController();
        var user = new User { Name = "Rahul", Age = 28, City = "Bengaluru", State = "Karnataka", Pincode = "560001" };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var result = await controller.DeleteUser(user.Id);

        Assert.IsType<NoContentResult>(result);
        Assert.Null(await context.Users.FindAsync(user.Id));
        await context.DisposeAsync();
    }

    [Fact]
    public async Task DeleteUser_WhenMissing_ReturnsNotFound()
    {
        var (context, controller) = CreateController();

        var result = await controller.DeleteUser(999);

        Assert.IsType<NotFoundObjectResult>(result);
        await context.DisposeAsync();
    }
}
