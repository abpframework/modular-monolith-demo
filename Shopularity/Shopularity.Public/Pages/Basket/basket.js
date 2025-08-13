(function ($) {
    var l = abp.localization.getResource('Shopularity');

    updateBasketCount = function (count) {
        var $basketCountArea = $('#shopularityBasketCount');
        var $basketCountLabel = $('#shopularityBasketItemCountLabel');
        var $BasketSunOnActions = $('#shopularityBasketSunOnActions');
        var $BasketEmpty= $('#shopularityBasketEmpty');

        if ($basketCountArea.length === 0 || $basketCountLabel.length === 0 || $BasketSunOnActions.length === 0) {
            return;
        }

        if (count === 0) {
            $basketCountArea.addClass('d-none')
                .text('');
            $BasketSunOnActions.addClass('d-none')
                .text('');
            $BasketEmpty
                .removeClass('d-none');
            $basketCountLabel.text('');
            return;
        }
        
        var displayCount = count > 99 ? '99+' : String(count);
        $basketCountArea.text(displayCount)
            .removeClass('d-none');
        $basketCountLabel.text(l('ProductWithCount{0}', displayCount))
            .removeClass('d-none');
        $BasketSunOnActions
            .removeClass('d-none');
        $BasketEmpty
            .addClass('d-none');
    }
    
    updateBasket = function () {
        var $basketArea = $('#shopularityBasketList');
        if ($basketArea.length === 0) {
            return;
        }
        const renderApi = '/api/basket/basket/render';
        $basketArea.load(renderApi);
    }
    
    abp.event.on('abp.serviceProxyScriptInitialized', function () {
        $(function () {
            if (abp.currentUser && abp.currentUser.id) {
                updateBasket();
                const connection = new signalR.HubConnectionBuilder().withUrl(abp.appPath + "signalr-hubs/basket").build();

                connection.on("BasketChange", function (newBasket) {
                    updateBasket();
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
