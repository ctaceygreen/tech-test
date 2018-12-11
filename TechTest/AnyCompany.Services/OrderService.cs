using AnyCompany.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyCompany.Services
{
    public class OrderService : IOrderService
    {
        IUoW _uow;
        ICustomerService _customerService;
        public OrderService(IUoW uow, ICustomerService customerService)
        {
            _uow = uow;
            _customerService = customerService;
        }
        public void AddOrder(OrderRequest orderRequest)
        {
            //if customer ID is null then call off to create customer first
            int customerId = -1;
            if (!orderRequest.CustomerId.HasValue || orderRequest.CustomerId == -1)
            {
                customerId = _customerService.AddCustomer(new CustomerRequest { Country = orderRequest.CustomerCountry, Name = orderRequest.CustomerName, DateOfBirth = orderRequest.DateOfBirth });
                
            }
            else
            {
                customerId = orderRequest.CustomerId.Value;
                Customer customer = _customerService.GetCustomer(customerId);
                orderRequest.CustomerName = customer.Name;
                orderRequest.CustomerCountry = customer.Country;
            }
            //Change order request into order object and add to uow
            Order order = new Order { Amount = orderRequest.Amount, CustomerId = customerId };
            if (order.Amount <= 0)
            {
                throw new Exception("Order amount must be greater than zero");
            }

            if (orderRequest.CustomerCountry == "UK")
                order.VAT = 0.2M;
            else
                order.VAT = 0;
            _uow.Orders.Create(order);
            _uow.Commit();
        }

    }
}
