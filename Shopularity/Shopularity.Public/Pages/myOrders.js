(function ($) {
    var l = abp.localization.getResource('Shopularity');

    $(".cancel-order").click(function () {
        var $button = $(this);
        var id = $(this).data('order-id');
        shopularity.public.controllers.shopularity.cancelOrder(id)
            .then(function () {
                $button.addClass("d-none")
                var $state = $button.closest('.order-state').find('.order-state-label');
                $state.text(l('Cancelled'));
                abp.notify.success(l('CancelledOrder'));
            })
            .catch(function (error) {
                console.error(error);
                abp.notify.error(l('ErrorCancellingOrder'));
            });
    });
    
})(jQuery);