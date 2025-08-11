(function ($) {
    var l = abp.localization.getResource('Shopularity');
    var productsPublicAppService = shopularity.public.controllers.products;

/*    productsPublicAppService.getList({}).then(function (result) {
        for (var i = 0; i < result.items.length; i++) {
            console.log(result.items[i].product.name);
        }
    });*/


    var basketAppService = shopularity.public.controllers.basket;
    $(".addToBasket").click(function () {
        var id = $(this).data('product-id');
        console.log(id);
        basketAppService.addItemToBasket({productId: id, amount: 1})
            .then(function (result) {
                abp.notify.success(l('AddedToBasket'));
            })
            .catch(function (error) {
                console.error(error);
                abp.notify.error(l('ErrorAddingToBasket'));
            });
    });

    var connection = new signalR.HubConnectionBuilder().withUrl(abp.appPath + "signalr-hubs/basket").build();

    connection.on("BasketChange", function (newBasket) {
        console.log(newBasket);
    });

    connection.start().then(function () {
    })
        .catch(function (err) {
            return console.error(err.toString());
        });
    
})(jQuery);
