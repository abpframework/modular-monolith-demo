using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Shopularity.Basket.Services;

public interface IBasketAppService: IApplicationService
{
    Task AddItemToBasket(BasketItem input);

    Task<List<BasketItem>> GetBasketItems();
}