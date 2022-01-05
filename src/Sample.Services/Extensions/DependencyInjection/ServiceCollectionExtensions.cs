using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using Sample.Services.Seed;
using Sample.Services.Users;

namespace Sample.Services.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSeedService(this IServiceCollection services)
    {
        services.AddScoped<ISeedService, SeedService>();

        return services;
    }

    public static IServiceCollection AddUserService(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}
