using Sitecore.Data.Items;
using Sitecore.Pipelines.HttpRequest;

namespace SitecoreAppl.Feature.Blog
{
    public class HttpRequestProcessor404 : HttpRequestProcessor
    {
        public override void Process(HttpRequestArgs args)
        {
            if (Sitecore.Context.Item != null || Sitecore.Context.Site == null || Sitecore.Context.Database == null
                || Sitecore.Context.Database.Name == "core"
                || args.RequestUrl.AbsoluteUri.ToLower().Contains("/sitecore/api/layout/render/jss")
                || args.RequestUrl.AbsoluteUri.ToLower().Contains("/sitecore/api/jss/import")
                || args.RequestUrl.AbsoluteUri.ToLower().Contains("/api/sitecore/"))
            {
                return;
            }
            var pageNotFound = Sitecore.Context.Database.GetItem(@"{CE776CE4-77CE-4229-A8E2-16648E0D54DD}");
            if (pageNotFound == null)
                return;
            args.ProcessorItem = pageNotFound;
            Sitecore.Context.Item = pageNotFound;

            args.HttpContext.Response.StatusCode = 404;
        }
    
}
}