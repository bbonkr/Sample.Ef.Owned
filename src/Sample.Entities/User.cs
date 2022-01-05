namespace Sample.Entities;

public class User
{
    public long Id { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public virtual ICollection<Address> Addresses { get; set; } = new HashSet<Address>();

    public virtual Metadata Metadata { get; set; } = new Metadata();

    public virtual Profile Profile { get; set; } = new Profile();
}
