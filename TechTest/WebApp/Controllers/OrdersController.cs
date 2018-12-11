using AnyCompany.Data;
using AnyCompany.OrderSendReceive;
using AnyCompany.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class OrdersController : ApiController
    {
        IOrderSender _orderSender;
        ICustomerService _customerService;
        public OrdersController(IOrderSender orderSender, ICustomerService customerService)
        {
            _orderSender = orderSender;
            _customerService = customerService;
        }

        // GET api/<controller>
        public IEnumerable<Customer> Get()
        {
            //Ordinarily we would not present Customer to the front end as it's in the Data project. However as this front end is simply to show the DB, I'm allowing it
            return _customerService.ListCustomersWithOrders();
        }
        
        // POST api/<controller>
        public void Post([FromBody]OrderRequestModel request)
        {
            _orderSender.SendOrder(new OrderRequest { Amount = request.amount, CustomerId = request.customerId == -1 ? (int?)null : request.customerId, CustomerCountry = request.customerCountry,DateOfBirth = DateTime.Parse(request.dateOfBirth), CustomerName = request.customerName });
        }
        
    }
}