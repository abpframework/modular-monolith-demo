(function ($) {
    var l = abp.localization.getResource('Shopularity');
    var productsPublicAppService = shopularity.public.controllers.products;

    productsPublicAppService.getList({}).then(function (result) {
        for (var i = 0; i < result.items.length; i++) {
            console.log(result.items[i].product.name);
        }
    });

})(jQuery);
