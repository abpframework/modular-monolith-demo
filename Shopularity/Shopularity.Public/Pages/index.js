(function ($) {
    var l = abp.localization.getResource('Shopularity');
    const renderApi = '/api/catalog/public/products/render';

    $("#ShopularityProductList").load(renderApi + "?skip=0&take=12");
    
    let skip = 12;
    $("#loadMore").on("click", function(){
        $.get(renderApi, { skip, take: 12, sorting: "Name" }, function(html){
            $("#productHost").append(html);
            skip += 12;
        });
    });
    
    var basketAppService = shopularity.public.controllers.basket;
    $(".addToBasket").click(function () {
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