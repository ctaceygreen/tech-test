using AnyCompany.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyCompany.OrderSendReceive
{
    public interface IOrderSender
    {
        void SendOrder(OrderRequest orderRequest);
    }
}
