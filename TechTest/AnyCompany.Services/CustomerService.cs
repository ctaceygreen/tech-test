using AnyCompany.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyCompany.Services
{
    public class CustomerService : ICustomerService
    {
        IUoW _uow;
        public CustomerService(IUoW uow)
        {
            _uow = uow;
        }

        public int AddCustomer(CustomerRequest customerRequest)
        {
            if (String.IsNullOrEmpty(customerRequest.Country))
            {
                throw new Exception("Customer country cannot be empty");
            }
            if (String.IsNullOrEmpty(customerRequest.Name))
            {
                throw new Exception("Customer name cannot be empty");
            }
            if(customerRequest.DateOfBirth == null || customerRequest.DateOfBirth == DateTime.MinValue)
            {
                throw new Exception("Date Of Birth cannot be empty");
            }
            Customer customer = new Customer { Name = customerRequest.Name, Country = customerRequest.Country, DateOfBirth = customerRequest.DateOfBirth };
            var dbCustomerId = _uow.Customers.Create(customer);
            _uow.Commit();
            return dbCustomerId;
        }

        public IEnumerable<Customer> ListCustomers()
        {
            return _uow.Customers.GetAll();
        }

        public IEnumerable<Customer> ListCustomersWithOrders()
        {
            return _uow.Customers.GetAllWithOrders();
        }
    }
}
