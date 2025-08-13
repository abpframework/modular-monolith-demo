(function ($) {
    var l = abp.localization.getResource('Shopularity');

    updateBasketCount = function (count) {
        var $basketCountArea = $('#ShopularityBasketCount');

        if ($basketCountArea.length === 0) {
            return;
        }

        if (count === 0) {
            $basketCountArea.addClass('d-none')
                .text('');
            return;
        }
        
        var displayCount = count > 99 ? '99+' : String(count);
        $basketCountArea.text(displayCount)
            .removeClass('d-none');
    }
    
    initBasketCount = function () {
        shopularity.public.controllers.basket.getBasketItems({}).then(function (result) {
            updateBasketCount(result.length);
        });
    }
    
    updateBasketItems = function () {
        var $basketArea = $('#ShopularityBasket');
        if ($basketArea.length === 0) {
            return;
        }
        const renderApi = '/api/basket/basket/render';
        $basketArea.load(renderApi);
    }
    
    abp.event.on('abp.serviceProxyScriptInitialized', function () {
        $(function () {
            if (abp.currentUser && abp.currentUser.id) {
                updateBasketItems();
                initBasketCount();
                const connection = new signalR.HubConnectionBuilder().withUrl(abp.appPath + "signalr-hubs/basket").build();

                connection.on("BasketChange", function (newBasket) {
                    updateBasketItems();
                    updateBasketCount(newBasket.length);
                });

                connection.start().then(function () {
                })
                    .catch(function (err) {
                        return console.error(err.toString());
                    });
                
            }
        });
    });
})(jQuery);
