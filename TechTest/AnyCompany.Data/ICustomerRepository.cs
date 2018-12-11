using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyCompany.Data
{
    public interface ICustomerRepository
    {
        int Create(Customer customer);
        Customer Load(int customerId);
        IEnumerable<Customer> GetAll();
        IEnumerable<Customer> GetAllWithOrders();
    }
}
