using CustomerOnboarding.Core.Models;

public class State
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = null!;

    // Navigation properties
    public ICollection<Lga> Lgas { get; set; } = new List<Lga>();
    public ICollection<Customer> Customers { get; set; } = new List<Customer>();
}
