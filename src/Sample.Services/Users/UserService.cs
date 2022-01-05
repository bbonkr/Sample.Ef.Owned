using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Sample.Data;
using Sample.Services.Seed;

namespace Sample.Services.Users;

public class UserService : IUserService
{
    public UserService(AppDbContext context, IMapper mapper, ILogger<SeedService> logger)
    {
        this.context = context;
        this.mapper = mapper;
        this.logger = logger;
    }

    public async Task<IEnumerable<UserModel>> GetUsersAsync(int page = 1, int limit = 10, string? keyword = null)
    {
        var query = context.Users
            .Where(x => x.Metadata.IsDeleted == false);

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            query = query
                .Where(x => x.FirstName.Contains(keyword) || x.LastName.Contains(keyword));
        }

        var skip = (page - 1) * limit;

        var usersQuery = query
            .OrderBy(x => x.FirstName).ThenBy(x => x.LastName)
            .Skip(skip).Take(limit)
            .Select(x => mapper.Map<UserModel>(x))
            .AsNoTracking();

        var verificationGeneratedQuery = usersQuery.ToQueryString();

        var users = await usersQuery.ToListAsync();

        return users;
    }

    public async Task<IEnumerable<UserModel>> GetUsersOtherWayAsync(int page = 1, int limit = 10, string? keyword = null)
    {
        var query = context.Users
           .Where(x => x.Metadata.IsDeleted == false);

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            query = query.Where(x => x.FirstName.Contains(keyword) || x.LastName.Contains(keyword));
        }

        var skip = (page - 1) * limit;

        var usersIdSubQuery = query
            .OrderBy(x => x.FirstName).ThenBy(x => x.LastName)
            .Skip(skip).Take(limit)
            .Select(x => x.Id);


        var usersMainQuery = context.Users
            .Where(x => usersIdSubQuery.Contains(x.Id))
            .Select(x => mapper.Map<UserModel>(x))
            .AsNoTracking();

        var verificationGeneratedQuery = usersMainQuery.ToQueryString();

        var users = await usersMainQuery.ToListAsync();

        return users;
    }

    private readonly AppDbContext context;
    private readonly IMapper mapper;
    private readonly ILogger logger;
}
