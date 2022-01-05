namespace Sample.Entities;

public class Address
{
    public long Id { get; set; }

    /// <summary>
    /// Name of address
    /// </summary>
    public string Name { get; set; } = string.Empty;

    public string Street { get; set; } = string.Empty;

    public string? Detail { get; set; }

    public string City { get; set; } = string.Empty;

    public string State { get; set; } = string.Empty;

    public string Country { get; set; } = string.Empty;

    public string Zipcode { get; set; } = string.Empty;
}