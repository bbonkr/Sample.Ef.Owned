using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Sample.Data;
using Sample.Entities;

namespace Sample.Services.Seed;

public class SeedService: ISeedService
{
    public SeedService(AppDbContext context, ILogger<SeedService> logger)
    {
        this.context = context;
        this.logger = logger;
    }

    public async Task ExecuteAsync()
    {
        if (!context.Users.Any())
        {
            context.AddRange(CreateUsers());
            await context.SaveChangesAsync();
        }
    }

    private IEnumerable<User> CreateUsers()
    {
        return new List<User>
        {
            CreateUser("John" , "Doe", CreateProfile(170, 60),CreateMetadata(), new HashSet<Address>{
                CreateAddress("Home", "1 Somewhere, Awesome street", "Ok city", "Yes state", "Hello world", "00000"),
            }),
            CreateUser("Jane", "Doe", CreateProfile(160, 50), CreateMetadata()),
            CreateUser("Bruce", "Wein", CreateProfile(188, 80), CreateMetadata(true)),
            CreateUser("Bruce", "Wein", CreateProfile(188, 80), CreateMetadata(true)),
            CreateUser("Tony", "Stark", CreateProfile(180, 70), CreateMetadata())
        };
    }

    private User CreateUser(string firstName, string lastName, Profile profile, Metadata metadata, ICollection<Address>? addresses = null)
    {
        return new User
        {
            FirstName = firstName,
            LastName = lastName,
            Profile = profile,
            Metadata = metadata,
            Addresses = addresses ?? new HashSet<Address>(),
        };
    }

    private Profile CreateProfile(double height, double weight)
    {
        return new Profile
        {
            Height = height,
            Weight = weight,
        };
    }

    private Metadata CreateMetadata(bool isDeleted = false)
    {
        return new Metadata
        {
            IsDeleted = isDeleted
        };
    }

    private Address CreateAddress(string name, string street, string city, string state, string country, string zipcode, string detail = null)
    {
        return new Address
        {
            Name = name,
            Street = street,
            Detail = detail,
            City = city,
            State = state,
            Country = country,
            Zipcode = zipcode,
        };
    }

    private readonly AppDbContext context;
    private readonly ILogger logger;
}
