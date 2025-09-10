using System;
using System.Collections.Generic;

namespace CustomerOnboarding.Core.Models
{
    public class Lga
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = null!;

        // Foreign key to State
        public Guid StateId { get; set; }
        public State State { get; set; } = null!;

        // Navigation property: Customers in this LGA
        public ICollection<Customer> Customers { get; set; } = new List<Customer>();
    }
}
