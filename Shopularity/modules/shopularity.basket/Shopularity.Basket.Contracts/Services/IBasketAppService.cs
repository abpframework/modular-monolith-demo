using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Shopularity.Basket.Services;

public interface IBasketAppService: IApplicationService
{
    //TODO: Remove "Basket" from method names
    Task AddItemAsync(AddBasketItemInput input); //TODO: Create specific DTO for this method

    Task RemoveItemAsync(RemoveBasketItemInput input); //TODO: Create specific DTO for this method
    
    Task<ListResultDto<BasketItemDto>> GetItemsAsync();
    
    Task<int> GetCountOfItemsAsync();
}