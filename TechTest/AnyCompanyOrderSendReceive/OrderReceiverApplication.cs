using AnyCompany.Services;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyCompany.OrderSendReceive
{
    public class OrderReceiverApplication
    {
        protected readonly IOrderService _orderService;
        public OrderReceiverApplication(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public void Run()
        {
            var queue = RabbitMQHelper.GetOrderQueueName();
            var factory = new ConnectionFactory() { HostName = RabbitMQHelper.GetServerHostname() };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: queue,
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    //Our body is the json of our orderRequest object.
                    var body = ea.Body;
                    string json = System.Text.Encoding.UTF8.GetString(ea.Body, 0, ea.Body.Length);
                    var orderRequest = JsonConvert.DeserializeObject<OrderRequest>(json);
                    _orderService.AddOrder(orderRequest);
                    Console.WriteLine(" [x] Received {0}", json);
                };
                channel.BasicConsume(queue: queue,
                                     autoAck: true,
                                     consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }

        }
    }
}
