using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnyCompany.Services;

namespace AnyCompany.OrderSendReceive
{
    public class NormalOrderSender : IOrderSender
    {
        IOrderService _orderService;
        public NormalOrderSender(IOrderService orderService)
        {
            _orderService = orderService;
        }
        public void SendOrder(OrderRequest orderRequest)
        {
            _orderService.AddOrder(orderRequest);
        }
    }
}
