namespace Sample.Services.Users;

public class UserModel
{
    public long Id { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public MetadataModel? Metadata { get; set; }

    public ProfileModel? Profile { get; set; }

    public IEnumerable<AddressModel>? Addresses { get; set; } = null;
}
