using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyCompany.Services
{
    public class OrderRequest
    {
        public int? CustomerId { get; set; } //If null, we are adding a new customer
        public string CustomerName { get; set; }
        public string CustomerCountry { get; set; }
        public decimal Amount { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
