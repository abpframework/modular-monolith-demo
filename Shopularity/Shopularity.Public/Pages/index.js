(function ($) {
    var l = abp.localization.getResource('Shopularity');
    const renderApi = '/api/catalog/public/products/render';

    $("#ShopularityProductList").load(renderApi + "?skip=0&maxResultCount=9");

    let skip = 9;
    let loading = false; // todo: show loading indicator
    let done = false;

    $(window).on("scroll", function() {
        if (loading || done) {
            return;
        }

        if ($(window).scrollTop() + $(window).height() >= $(document).height() - 100) {
            loading = true;

            $.get(renderApi, {skipCount: skip, maxResultCount: 6}, function(html) {
                if ($.trim(html).length === 0) {
                    done = true;
                    loading = false;
                    return;
                }

                $("#ShopularityProductList").append(html);
                skip += 6;
                loading = false;
            });
        }
    });
    
    var basketAppService = shopularity.public.controllers.basket;
    $(document).on("click", ".addToBasket", function () {
        var id = $(this).data('product-id');
        basketAppService.addItemToBasket({productId: id, amount: 1})
            .then(function (result) {
                abp.notify.success(l('AddedToBasket'));
            })
            .catch(function (error) {
                console.error(error);
                abp.notify.error(l('ErrorAddingToBasket'));
            });
    });
    
})(jQuery);