namespace AnyCompany.Data
{
    public class Order
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public decimal VAT { get; set; }
        public int CustomerId { get; set; }
    }
}
