using AnyCompany.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyCompany.Services
{
    public interface IOrderService
    {
        void AddOrder(OrderRequest orderRequest);
    }
}
