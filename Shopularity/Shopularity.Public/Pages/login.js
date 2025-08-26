(function ($) {
    var l = abp.localization.getResource('Shopularity');

    $(document).on("click", ".button-login-required", function (e) {
        e.preventDefault();

        abp.message.info(
            l('LoginRequiredToContinueText'),
            l('LoginRequiredToContinueTitle')
        ).then(function () {
            window.location.href = "/account/login";
        });
    });
    
})(jQuery);