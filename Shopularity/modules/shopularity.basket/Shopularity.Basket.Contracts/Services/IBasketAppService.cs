using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Shopularity.Basket.Services;

public interface IBasketAppService: IApplicationService
{
    Task AddItemAsync(AddBasketItemInput input);

    Task RemoveItemAsync(RemoveBasketItemInput input);
    
    Task<ListResultDto<BasketItemDto>> GetItemsAsync();
    
    Task<int> GetCountOfItemsAsync();
}