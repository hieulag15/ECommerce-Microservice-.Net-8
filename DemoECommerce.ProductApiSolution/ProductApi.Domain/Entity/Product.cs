namespace ProductApi.Domain.Entity;

public class Product
{
    public Guid Id { get; set; }
    public String? Name { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}
