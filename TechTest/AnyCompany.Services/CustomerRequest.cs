﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyCompany.Services
{
    public class CustomerRequest
    {
        public string Name { get; set; }
        public string Country { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
