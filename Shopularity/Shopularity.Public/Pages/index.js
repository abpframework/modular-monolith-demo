(function ($) {
    var l = abp.localization.getResource('Shopularity');
    const renderApi = '/api/catalog/public/products/render';
    var productsPublicAppService = shopularity.public.controllers.products;
    
    $("#ShopularityProductList").load(renderApi + "?skip=0&maxResultCount=12");
    
    let skip = 12;
    $("#loadMore").on("click", function(){
        $.get(renderApi, { skip, maxResultCount: 12, sorting: "Name" }, function(html){
            $("#productHost").append(html);
            skip += 12;
        });
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