using Shopularity.Basket.Services;

namespace Shopularity.Services.Dtos;

public class NewOrderInputDto
{
    public List<BasketItem> Products { get; set; }
    
    public string Address { get; set; }
    
    public string CreditCardNo { get; set; }
}