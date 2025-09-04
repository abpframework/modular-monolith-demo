(function ($) {
    var l = abp.localization.getResource('Shopularity');

    var basketAppService = shopularity.public.controllers.basket;
    $(document).on("click", ".addToBasket", function () {
        var id = $(this).data('product-id');
        basketAppService.addItem({itemId: id, amount: 1})
            .then(function (result) {
                abp.notify.success(l('AddedToBasket'));
            });
    });
    
})(jQuery);