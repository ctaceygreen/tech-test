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
        private ISqlDataContext _context;
        public CustomerRepositoryUmbrella(ISqlDataContext context)
        {
            _context = context;
        }

        public Customer Load(int customerId)
        {
            List<Customer> customers = new List<Customer>();
            using (var command = _context.CreateCommand())
            {
                command.CommandText = CustomerRepository.CreateCommandGetCustomer();
                command.Parameters.Add(_context.CreateParameter("@Id", customerId));
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    customers.Add(CustomerFromReader(reader, false));
                }
                reader.Close();
            }
            _context.SaveChanges();
            return customers.FirstOrDefault();
        }

        public int Create(Customer customer)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandText = CustomerRepository.CreateCommandCreateCustomer();
                command.Parameters.Add(_context.CreateParameter("@Name", customer.Name));
                command.Parameters.Add(_context.CreateParameter("@Country", customer.Country));
                command.Parameters.Add(_context.CreateParameter("@DateOfBirth", customer.DateOfBirth.ToString("yyyy-MM-dd")));
                command.ExecuteNonQuery();
                object returnObj = command.ExecuteScalar();
                int id = int.Parse(returnObj.ToString());
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
                    customers.Add(CustomerFromReader(reader, false));
                }
                reader.Close();
            }
            return customers;
        }

        public Customer CustomerFromReader(IDataReader reader, bool usingJoin)
        {
            string customerId = usingJoin ? "Customer_Id" : "Id";
            return new Customer
            {
                Name = reader["Name"].ToString(),
                DateOfBirth = DateTime.Parse(reader["DateOfBirth"].ToString()),
                Country = reader["Country"].ToString(),
                Id = int.Parse(reader[customerId].ToString())
            };
        }

        public IEnumerable<Customer> GetAllWithOrders()
        {
            Dictionary<int, Customer> ordersByCustomer = new Dictionary<int, Customer>();
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = CustomerRepository.CreateCommandGetAllWithOrders();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    // Create customer from customer data
                    var customer = CustomerFromReader(reader, true); 
                    // Create order from order data
                    var order = new Order
                    {
                        Amount = decimal.Parse(reader["Amount"].ToString()),
                        CustomerId = customer.Id,
                        Id = int.Parse(reader["Order_Id"].ToString()),
                        VAT = decimal.Parse(reader["VAT"].ToString())
                    }; 

                    if (ordersByCustomer.ContainsKey(customer.Id))
                    {
                        //Then just add this order data
                        ordersByCustomer[customer.Id].Orders.Add(order);
                    }
                    else
                    {
                        //Not seen this customer before so add new
                        //Clone customer for key, so we're not hashing on orders too
                        customer.Orders.Add(order);
                        ordersByCustomer.Add(customer.Id, customer);
                    }
                }
                reader.Close();
            }
            return ordersByCustomer.Select(c => c.Value);
        }
    }
}
