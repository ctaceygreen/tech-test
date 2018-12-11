using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyCompany.OrderSendReceive
{
    public static class RabbitMQHelper
    {
        public static string GetServerHostname() => "localhost";
        public static string GetOrderQueueName() => "orders";
    }
}
