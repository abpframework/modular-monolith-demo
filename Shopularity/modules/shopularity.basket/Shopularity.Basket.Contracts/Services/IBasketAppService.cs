using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Shopularity.Basket.Services;

public interface IBasketAppService: IApplicationService
{
    Task AddItemToBasketAsync(BasketItem input);

    Task RemoveItemFromBasketAsync(BasketItem input);
    
    Task<ListResultDto<BasketItemDto>> GetBasketItemsAsync();
    
    Task<int> GetCountOfItemsInBasketAsync();
}