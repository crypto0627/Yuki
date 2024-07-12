namespace GrpcServices.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string PurchaseDate { get; set; }
        public string ProductName { get; set; }
        public double PurchaseAmount { get; set; }
    }
}