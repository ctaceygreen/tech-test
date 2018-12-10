using System;
using System.Data.SqlClient;

namespace AnyCompany.Data
{
    //Since the rules specified that this class had to remain static, but I definitely didn't want a static repository, this is the tiny job I've given it instead!
    public static class CustomerRepository
    {
        public static string CreateCommandCreateCustomer()
        {
            return "INSERT INTO Customers VALUES (@Name, @Country)";
        }

        public static string CreateCommandGetAll()
        {
            return @"SELECT * FROM Customers
                    ORDER BY Customers.CustomerId";
        }

        public static string CreateCommandGetAllWithOrders()
        {
            return @"SELECT * FROM Customers 
                    INNER JOIN Orders 
                    on Customers.Id = Orders.CustomerId
                    ORDER BY Customers.CustomerId";
        }
    }
}
