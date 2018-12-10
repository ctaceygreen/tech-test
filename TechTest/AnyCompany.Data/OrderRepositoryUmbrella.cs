using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyCompany.Data
{
    class OrderRepositoryUmbrella : IOrderRepository
    {
        private AdoContext _context;
        public OrderRepositoryUmbrella(AdoContext context)
        {
            _context = context;
        }
        public void Create(Order order)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandText = OrderRepository.CreateCommandCreateOrder();
                command.Parameters.Add(_context.CreateParameter("@Amount", order.Amount));
                command.Parameters.Add(_context.CreateParameter("@VAT", order.VAT));
                command.Parameters.Add(_context.CreateParameter("@CustomerId", order.CustomerId));
            }
        }
    }
}
