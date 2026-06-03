namespace RetailOptimizationPlatform.Exceptions
{
    public class ProductNotFoundException : OrderProcessingException
    {
        public ProductNotFoundException() : base("Product not found.")
        {
        }
    }
}
