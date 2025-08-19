(function ($) {
    var l = abp.localization.getResource('Shopularity');

    var basketAppService = shopularity.public.controllers.basket;
    $(document).on("click", ".addToBasket", function () {
        var id = $(this).data('product-id');
        basketAppService.addItemToBasket({itemId: id, amount: 1})
            .then(function (result) {
                abp.notify.success(l('AddedToBasket'));
            })
            .catch(function (error) {
                console.error(error);
                abp.notify.error(l('ErrorAddingToBasket'));
            });
    });
    
})(jQuery);