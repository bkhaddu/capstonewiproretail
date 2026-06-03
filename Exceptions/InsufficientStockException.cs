namespace RetailOptimizationPlatform.Exceptions
{
    public class InsufficientStockException : OrderProcessingException
    {
        public InsufficientStockException(string productName)
            : base($"Insufficient stock for {productName}")
        {
        }
    }
}
