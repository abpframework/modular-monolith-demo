using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Shopularity.Basket.Services;
using Volo.Abp.DependencyInjection;

namespace Shopularity.Public.SignalR;

public class ExternalBasketHubService : BackgroundService, ISingletonDependency
{
    private readonly IHubContext<BasketHub> _basketHub;
    private readonly IConfiguration _configuration;
    private HubConnection? _hubConnection;

    public ExternalBasketHubService(
        IHubContext<BasketHub> basketHub,
        IConfiguration configuration)
    {
        _basketHub = basketHub;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var externalHubUrl = $"{_configuration["RemoteServices:Default:BaseUrl"]}/signalr-hubs/basket";

        _hubConnection = new HubConnectionBuilder()
            .WithUrl(externalHubUrl)
            .WithAutomaticReconnect()
            .Build();

        _hubConnection.On<BasketUpdatedEto>("BasketUpdated", async item =>
        {
            await _basketHub.Clients.User(item.UserId.ToString()).SendAsync("BasketUpdated", item);
        });

        await _hubConnection.StartAsync(stoppingToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_hubConnection is not null)
        {
            await _hubConnection.DisposeAsync();
        }
        
        await base.StopAsync(cancellationToken);
    }
}
