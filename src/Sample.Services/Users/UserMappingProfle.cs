using Sample.Entities;

namespace Sample.Services.Users;

public class UserMappingProfle : AutoMapper.Profile
{
    public UserMappingProfle()
    {
        CreateMap<User, UserModel>();
        CreateMap<Address, AddressModel>();
        CreateMap<Metadata, MetadataModel>();
        CreateMap<Entities.Profile, ProfileModel>();
    }
}