using AnyCompany.Services;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyCompany.OrderSendReceive
{
    public class RabbitOrderSender : IOrderSender
    {
        public RabbitOrderSender()
        {
        }
        public void SendOrder(OrderRequest orderRequest)
        {
            var factory = new ConnectionFactory() { HostName = RabbitMQHelper.GetServerHostname() };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: RabbitMQHelper.GetOrderQueueName(),
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var body = JsonConvert.SerializeObject(orderRequest);

                channel.BasicPublish(exchange: "",
                                     routingKey: RabbitMQHelper.GetOrderQueueName(),
                                     basicProperties: null,
                                     body: System.Text.Encoding.UTF8.GetBytes(body),
                                     mandatory: true);
            }

        }
    }
}

