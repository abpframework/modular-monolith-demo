(function ($) {
    abp.event.on('abp.serviceProxyScriptInitialized', function () {
        $(function () {
            if (abp.currentUser && abp.currentUser.id) {

                const connection = new signalR.HubConnectionBuilder().withUrl(abp.appPath + "signalr-hubs/basket").build();
                var productsPublicAppService = shopularity.public.controllers.products;

                connection.on("BasketChange", function (newBasket) {
                    for (let i = 0; i < newBasket.length; i++) {
                        productsPublicAppService.get(newBasket[i].productId)
                            .then(function (result) {
                                console.log(result);
                            })
                            .catch(function (error) {
                                console.error("Error fetching product:", error);
                            });
                    }
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
