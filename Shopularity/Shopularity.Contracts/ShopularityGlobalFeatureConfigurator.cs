using Volo.Abp.GlobalFeatures;
using Volo.Abp.Threading;

namespace Shopularity;

public static class ShopularityGlobalFeatureConfigurator
{
    private static readonly OneTimeRunner OneTimeRunner = new ();

    public static void Configure()
    {
        OneTimeRunner.Run(() =>
        {
            GlobalFeatureManager.Instance.Modules.CmsKit(cmsKit =>
            {
                cmsKit.Comments.Enable();
                cmsKit.Ratings.Enable();
            });  
        });
    }
}
