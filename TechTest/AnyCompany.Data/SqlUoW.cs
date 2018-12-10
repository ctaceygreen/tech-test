using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyCompany.Data
{
    public class SqlUoW : IUoW
    {
        const string ConnectionStringConfig = "DefaultConnection";
        private AdoContext _context;
        private OrderRepositoryUmbrella _orders;
        private CustomerRepositoryUmbrella _customers;

        public SqlUoW()
        {
            var connectionString =
                             ConfigurationManager
                             .ConnectionStrings[ConnectionStringConfig]
                             .ConnectionString;
            _context = new AdoContext(connectionString);
        }

        public IOrderRepository Orders
        {
            get
            {
                return _orders ?? (_orders = new OrderRepositoryUmbrella(_context));
            }
        }

        public ICustomerRepository Customers
        {
            get
            {
                return _customers ?? (_customers = new CustomerRepositoryUmbrella(_context));
            }
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
