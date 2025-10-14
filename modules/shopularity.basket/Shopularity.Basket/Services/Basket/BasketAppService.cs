using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Shopularity.Basket.Domain.Basket;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Users;

namespace Shopularity.Basket.Services.Basket;

[Authorize]
public class BasketAppService : BasketAppServiceBase, IBasketAppService
{
    private readonly BasketManager _basketManager;

    public BasketAppService(BasketManager basketManager)
    {
        _basketManager = basketManager;
    }

    public async Task AddItemAsync(AddBasketItemInput input)
    {
        await _basketManager.AddItemAsync(CurrentUser.GetId(), input.ItemId, input.Amount);
    }

    public async Task RemoveItemAsync(RemoveBasketItemInput input)
    {
        await _basketManager.RemoveItemsAsync(CurrentUser.GetId(), new Dictionary<Guid, int>
        {
            { input.ItemId, input.Amount }
        });
    }

    public async Task<ListResultDto<BasketItemDto>> GetItemsAsync()
    {
        var items = await _basketManager.GetItemsAsync(CurrentUser.GetId());
        
        return new ListResultDto<BasketItemDto>(
            ObjectMapper.Map<List<BasketItemWithProductInfo>, List<BasketItemDto>>(items)
            );
    }

    public async Task<int> GetCountOfItemsAsync()
    {
        return await _basketManager.GetCountOfItemsAsync(CurrentUser.GetId());
    }
}