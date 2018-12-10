using System.Data.SqlClient;

namespace AnyCompany.Data
{
    //To remain consistent with the CustomerRepository being a static class, OrderRepository will be the same
    internal class OrderRepository
    {
        public static string CreateCommandCreateOrder()
        {
            return "INSERT INTO Orders VALUES (@Amount, @VAT, @CustomerId)";
        }
    }
}
