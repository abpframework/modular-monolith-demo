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
    
    updateBasket = function (items) {
        var productsPublicAppService = shopularity.public.controllers.products;
        var $basketArea = $('#shopularityBasketList');
        var $BasketTotal= $('#shopularityBasketTotal');
        $BasketTotal.text('');

        if ($basketArea.length === 0) {
            return;
        }
        
        $basketArea.empty();
        var totalPrice = 0.00;
        for (let i = 0; i < items.length; i++) {
            productsPublicAppService.get(items[i].productId)
                .then(function (result) {
                    var $item = $('<div class="row" style="padding: 10px; display: flex; align-items: center;width: 300px"></div>');
                    $item.append('<img src="data:image/png;base64,' + result.product.image + '" alt="' + result.product.name + '" style="width: 80px; height: auto; margin-right: 15px; border-radius: 4px;">');
                    var $info = $('<div></div>');
                    $info.append('<div class="product-name" style="font-weight: bold;">' + result.product.name + '</div>');
                    $info.append('<div class="product-price" style="color: green;">$' + result.product.price + '</div>');
                    $info.append('<div class="product-amount" style="color: gray;">Amount: ' + items[i].amount + '</div>');
                    $item.append($info);
                    $basketArea.append($item);
                    
                    totalPrice += result.product.price * items[i].amount;
                    $BasketTotal.text('$' + totalPrice.toFixed(2));
                })
                .catch(function (error) {
                    console.error("Error fetching product:", error);
                });
        }
        
        updateBasketCount(items.length);
    }
    
    initBasket = function () {
        var basketAppService = shopularity.public.controllers.basket;

        basketAppService.getBasketItems()
            .then(function (result) {
                updateBasket(result);
            })
            .catch(function (error) {
                console.error("Error fetching basket:", error);
            });
    }
    
    abp.event.on('abp.serviceProxyScriptInitialized', function () {
        $(function () {
            if (abp.currentUser && abp.currentUser.id) {
                initBasket();
                const connection = new signalR.HubConnectionBuilder().withUrl(abp.appPath + "signalr-hubs/basket").build();

                connection.on("BasketChange", function (newBasket) {
                    updateBasket(newBasket);
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
