namespace PriseDeCommande.Class
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public byte[] ImageData { get; set; }
        public float Price { get; set; }
        public int Quantity { get; set; }
        
    }
}