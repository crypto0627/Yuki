namespace GrpcServices.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int ProductAmount { get; set; }
        public string Supplier { get; set; }
        public string ProductDetails { get; set; }
        public double ProductPrice { get; set; }
        public string IssueDate { get; set; }
    }
}