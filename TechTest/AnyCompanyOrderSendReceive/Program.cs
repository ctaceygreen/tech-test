using AnyCompany.Services;
using AnyCompany.Services.IoC;
using AnyCompanyOrderSendReceive.IoC;
using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyCompany.OrderSendReceive
{
    class Program
    {
        static private IContainer CompositionRoot()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<OrderReceiverApplication>();
            builder.RegisterModule<ServicesModule>();
            builder.RegisterModule<OrderSendModule>();
            return builder.Build();
        }

        static public void Main()
        {
            CompositionRoot().Resolve<OrderReceiverApplication>().Run();
        }
    }
}
