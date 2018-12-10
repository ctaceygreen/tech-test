using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyCompany.Data
{
    public interface IUoW : IDisposable
    {
        IOrderRepository Orders { get; }
        ICustomerRepository Customers { get; }
        void Commit();
    }
}
