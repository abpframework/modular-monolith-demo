(function ($) {
    var l = abp.localization.getResource('Shopularity');

    $(document).on('click', '.remove-from-basket', function() {
        var $container = $(this).closest('.basket-item').find('.amount-control');
        var $value = $container.find('.amount-value');
        var amount = parseInt($value.text(), 10) || 0;

        shopularity.public.controllers.basket.removeItemFromBasket({
            itemId: $container.data('product-id'),
            amount: amount
        })
            .then(function (result) {
                // let signalr update ui
            })
            .catch(function (error) {
                abp.notify.error(l('ErrorRemovingFromBasket'));
            });
    });

    $(document).on('click', '.amount-plus', function() {
        var $container = $(this).closest('.amount-control');

        shopularity.public.controllers.basket.addItemToBasket({
            itemId: $container.data('product-id'),
            amount: 1
            })
            .then(function (result) {
                // let signalr update ui
            })
            .catch(function (error) {
                abp.notify.error(l('ErrorAddingToBasket'));
            });
    });

    $(document).on('click', '.amount-minus', function() {
        var $container = $(this).closest('.amount-control');

        shopularity.public.controllers.basket.removeItemFromBasket({
            itemId: $container.data('product-id'),
            amount: 1
            })
            .then(function (result) {
                // let signalr update ui
            })
            .catch(function (error) {
                abp.notify.error(l('ErrorRemovingFromBasket'));
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
        shopularity.public.controllers.basket.getCountOfItemsInBasket({}).then(function (result) {
            updateBasketCount(result);
        });
    }
    
    renderBasketItems = function () {
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
                renderBasketItems();
                initBasketCount();
                const connection = new signalR.HubConnectionBuilder().withUrl(abp.remoteServiceUrl + "/signalr-hubs/basket").build();

                connection.on("BasketUpdated", function (result) {
                    renderBasketItems();
                    updateBasketCount(result.itemCountInBasket);
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
