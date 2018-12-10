using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyCompany.Data
{
    public interface ICustomerRepository
    {
        void Create(Customer customer);
        IEnumerable<Customer> GetAll();
        IEnumerable<Customer> GetAllWithOrders();
    }
}
