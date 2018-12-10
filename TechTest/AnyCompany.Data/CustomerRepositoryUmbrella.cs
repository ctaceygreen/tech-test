using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyCompany.Data
{
    public class CustomerRepositoryUmbrella : ICustomerRepository
    {
        private AdoContext _context;
        public CustomerRepositoryUmbrella(AdoContext context)
        {
            _context = context;
        }

        public int Create(Customer customer)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandText = CustomerRepository.CreateCommandCreateCustomer();
                command.Parameters.Add(_context.CreateParameter("@Name", customer.Name));
                command.Parameters.Add(_context.CreateParameter("@Country", customer.Country));
                command.Parameters.Add(_context.CreateParameter("@DateOfBirth", customer.DateOfBirth));
                var returnParam = _context.CreateOutputParameter("@Id", SqlDbType.Int);
                command.Parameters.Add(returnParam);
                command.ExecuteNonQuery();
                int id = (int)returnParam.Value;
                return id;
            }
        }

        public IEnumerable<Customer> GetAll()
        {
            List<Customer> customers = new List<Customer>();
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = CustomerRepository.CreateCommandGetAll();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    customers.Add(new Customer { });
                }
            }
            return customers;
        }

        public IEnumerable<Customer> GetAllWithOrders()
        {
            Dictionary<Customer, Customer> ordersByCustomer = new Dictionary<Customer, Customer>();
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = CustomerRepository.CreateCommandGetAllWithOrders();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var customer = new Customer { }; // Create customer from customer data
                    var order = new Order { }; // Create order from order data
                    if (ordersByCustomer.ContainsKey(customer))
                    {
                        //Then just add this order data
                        ordersByCustomer[customer].Orders.Add(order);
                    }
                    else
                    {
                        //Not seen this customer before so add new
                        var customerWithOrder = customer;
                        customerWithOrder.Orders.Add(order);
                        ordersByCustomer.Add(customer, customerWithOrder);
                    }
                }
            }
            return ordersByCustomer.Select(c => c.Value);
        }
    }
}
