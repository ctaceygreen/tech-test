using System;
using System.Collections;
using System.Collections.Generic;

namespace AnyCompany.Data
{
    public class Customer
    {
        public int Id { get; set; }

        public string Country { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Name { get; set; }

        public ICollection<Order> Orders = new HashSet<Order>();
    }
}
