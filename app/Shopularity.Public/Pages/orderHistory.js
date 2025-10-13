(function ($) {
    var l = abp.localization.getResource('Shopularity');

    $(".cancel-order").click(function () {
        var $button = $(this);
        var id = $(this).data('order-id');
        
        shopularity.public.controllers.ordering.cancel(id)
            .then(function () {
                $button.addClass("d-none")
                var $state = $($($button.parent()).parent().parent()).find('.order-state-label');
                $state.text(l('Cancelled'));
                abp.notify.success(l('CancelledOrder'));
            })
            .catch(function (error) {
                console.error(error);
                abp.notify.error(l('ErrorCancellingOrder'));
            });
    });
    
})(jQuery);