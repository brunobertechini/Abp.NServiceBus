using Abp.Web.Mvc.Views;

namespace Abp.NServiceBus.Web.Views
{
    public abstract class NServiceBusWebViewPageBase : NServiceBusWebViewPageBase<dynamic>
    {

    }

    public abstract class NServiceBusWebViewPageBase<TModel> : AbpWebViewPage<TModel>
    {
        protected NServiceBusWebViewPageBase()
        {
            LocalizationSourceName = NServiceBusConsts.LocalizationSourceName;
        }
    }
}