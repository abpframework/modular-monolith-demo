(function ($) {
    let params = new URLSearchParams(window.location.search);
    var l = abp.localization.getResource('Shopularity');
    const renderApi = '/api/catalog/public/products/render';
    var $shopularityProductList = $("#ShopularityProductList");
    let selectedCategory = '';
    if (params.get("category") != null){
        selectedCategory = params.get("category");
        $(".category-item[data-category-name='"+selectedCategory+"']").find('.category-item-text').addClass("selected-category");
    }
    
    $shopularityProductList.load(renderApi + "?skip=0&maxResultCount=9&categoryName=" + encodeURIComponent(selectedCategory));

    let skip = 9;
    let loading = false;
    let done = false;

    $(window).on("scroll", function() {
        if (loading || done) {
            return;
        }

        if ($(window).scrollTop() + $(window).height() >= $(document).height() - 100) {
            loading = true;

            $.get(renderApi, {skipCount: skip, maxResultCount: 6, categoryName: selectedCategory}, function(html) {
                if ($.trim(html).length === 0) {
                    done = true;
                    loading = false;
                    return;
                }

                $shopularityProductList.append(html);
                skip += 6;
                loading = false;
            });
        }
    });
    
    $(document).on("click", ".category-item", function () {
        var $this = $(this);
        $('.category-item-text').removeClass("selected-category");
        $this.find('.category-item-text').addClass("selected-category");
        selectedCategory = $this.data('category-name');
        skip = 0;
        done = false;
        $.get(renderApi, {skipCount: skip, maxResultCount: 6, categoryName: selectedCategory}, function(html) {
            if ($.trim(html).length === 0) {
                done = true;
                loading = false;
                return;
            }

            $shopularityProductList.empty();
            $shopularityProductList.append(html);
            skip += 6;
            loading = false;
            
            if (selectedCategory === undefined){
                window.history.replaceState({}, "", window.location.pathname);
            }
            else{
                params.set("category", selectedCategory);
                let newUrl = window.location.pathname + "?" + params.toString();
                window.history.pushState({}, "", newUrl);
            }
        });
    });
    
})(jQuery);