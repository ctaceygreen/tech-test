using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class OrderRequestModel
    {
        public int customerId { get; set; }
        public string customerName { get; set; }
        public string customerCountry { get; set; }
        public string dateOfBirth { get; set; }
        public decimal amount { get; set; }
    }
}