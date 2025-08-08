(function ($) {
    var l = abp.localization.getResource('Shopularity');
    var productsPublicAppService = shopularity.public.controllers.products;

    productsPublicAppService.getList({}).then(function (result) {
        for (var i = 0; i < result.items.length; i++) {
            console.log(result.items[i].product.name);
        }
    });


    var connection = new signalR.HubConnectionBuilder().withUrl(abp.appPath + "signalr-hubs/basket").build();
    
})(jQuery);
