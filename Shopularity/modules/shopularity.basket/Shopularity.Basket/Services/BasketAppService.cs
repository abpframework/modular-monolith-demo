using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Shopularity.Basket.Domain;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Users;

namespace Shopularity.Basket.Services;

[Authorize]
public class BasketAppService : BasketAppServiceBase, IBasketAppService
{
    private readonly BasketManager _basketManager;

    public BasketAppService(BasketManager basketManager)
    {
        _basketManager = basketManager;
    }

    public async Task AddItemAsync(BasketItem input)
    {
        await _basketManager.AddItemAsync(CurrentUser.GetId(), input);
    }

    public async Task RemoveItemAsync(BasketItem input)
    {
        await _basketManager.RemoveItemsAsync(CurrentUser.GetId(), [input]);
    }

    public async Task<ListResultDto<BasketItemDto>> GetItemsAsync()
    {
        return new ListResultDto<BasketItemDto>(
            await _basketManager.GetItemsAsync(CurrentUser.GetId())
            );
    }

    public async Task<int> GetCountOfItemsAsync()
    {
        return await _basketManager.GetCountOfItemsAsync(CurrentUser.GetId());
    }
}