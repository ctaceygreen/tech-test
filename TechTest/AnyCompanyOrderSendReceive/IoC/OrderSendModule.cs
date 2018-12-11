using AnyCompany.OrderSendReceive;
using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyCompanyOrderSendReceive.IoC
{
    public class OrderSendModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RabbitOrderSender>().As<IOrderSender>();
        }
    }
}
