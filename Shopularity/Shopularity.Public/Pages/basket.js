(function ($) {
    var l = abp.localization.getResource('Shopularity');

    $(document).on('click', '.remove-from-basket', function() {
        var $container = $(this).closest('.basket-item').find('.amount-control');
        var $value = $container.find('.amount-value');
        var amount = parseInt($value.text(), 10) || 0;

        shopularity.public.controllers.basket.removeItemFromBasket({
            productId: $container.data('product-id'),
            amount: amount
        })
            .then(function (result) {
                // let signalr update ui
            });
    });

    $(document).on('click', '.amount-plus', function() {
        var $container = $(this).closest('.amount-control');

        shopularity.public.controllers.basket.addItemToBasket({
            productId: $container.data('product-id'),
            amount: 1
        })
            .then(function (result) {
                // let signalr update ui
        });
    });

    $(document).on('click', '.amount-minus', function() {
        var $container = $(this).closest('.amount-control');

        shopularity.public.controllers.basket.removeItemFromBasket({
            productId: $container.data('product-id'),
            amount: 1
        })
            .then(function (result) {
                // let signalr update ui
            });
    });

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
    
    renderBasketItems = function () {
        var $basketArea = $('#ShopularityBasket');
        var $basketPageListArea = $('#ShopularityBasketPageList');
        if ($basketArea.length === 0) {
            return;
        }
        const renderApi = '/api/basket/basket/render';
        $basketArea.load(renderApi);
        $basketPageListArea.load(renderApi + '?isBasketPage=true');
    }
    
    abp.event.on('abp.serviceProxyScriptInitialized', function () {
        $(function () {
            if (abp.currentUser && abp.currentUser.id) {
                renderBasketItems();
                initBasketCount();
                const connection = new signalR.HubConnectionBuilder().withUrl(abp.appPath + "signalr-hubs/basket").build();

                connection.on("BasketItemAdded", function (result) {
                    renderBasketItems(result.item);
                    updateBasketCount(result.remainingItemCountInBasket);
                });
                
                connection.on("BasketItemRemoved", function (result) {
                    renderBasketItems(result.item);
                    updateBasketCount(result.remainingItemCountInBasket);
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
