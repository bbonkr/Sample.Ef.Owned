using Microsoft.AspNetCore.Mvc;

using Sample.Services.Users;

namespace Sample.App.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersOtherWayController : ControllerBase
{
    public UsersOtherWayController(IUserService userService, ILogger<UsersController> logger)
    {
        this.userService = userService;
        this.logger = logger;
    }

    [HttpGet]
    public async Task<IEnumerable<UserModel>> GetUsers(int page = 1, int limit = 10, string? keyword = "")
    {
        var users = await userService.GetUsersOtherWayAsync(page, limit, keyword);

        return users;
    }

    private readonly IUserService userService;
    private readonly ILogger logger;
}