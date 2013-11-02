using System.Linq;
using System.Text;
using System.Web.Mvc;
using Arana.DoubleClickForPublishers.Models;
using Arana.DoubleClickForPublishers.Services;
using Orchard;
using Orchard.ContentManagement;
using Orchard.DisplayManagement;
using Orchard.Environment.Extensions;
using Orchard.Mvc.Filters;
using Orchard.UI.Resources;

namespace Arana.DoubleClickForPublishers.Filters {
   [OrchardFeature("Arana.DoubleClickForPublishers")]
   public class DoubleClickForPublishersFilter : FilterProvider, IResultFilter {
      private readonly IResourceManager _resourceManager;
      private readonly IOrchardServices _orchardServices;
      private readonly IAdService _adService;
      private readonly IWorkContextAccessor _workContextAccessor;

      public DoubleClickForPublishersFilter(IResourceManager resourceManager, IOrchardServices orchardServices,
                                            IAdService adService, IShapeFactory shape, IWorkContextAccessor workContextAccessor) {
         _resourceManager = resourceManager;
         _orchardServices = orchardServices;
         _adService = adService;
         _workContextAccessor = workContextAccessor;
         Shape = shape;
      }

      public dynamic Shape { get; set; }
      #region IResultFilter Members

      public void OnResultExecuting(ResultExecutingContext filterContext) {
         var viewResult = filterContext.Result as ViewResult;
         if (viewResult == null) {
            return;
         }

         //Determine if we're on an admin page
         bool isAdmin = Orchard.UI.Admin.AdminFilter.IsApplied(filterContext.RequestContext);

         if (isAdmin) {
            return; // Not a valid configuration, ignore filter
         }

         var part = _orchardServices.WorkContext.CurrentSite.As<DoubleClickForPublishersSettingsPart>();
         if (part == null) {
            return; // Not a valid configuration, ignore filter
         }

         var layout = _workContextAccessor.GetContext( filterContext ).Layout;
         var headShape = Shape.DoubleClickForPublishersHeadScript( UseAsyncTags: part.UseAsyncTags );
         layout.Zones.Head.Add( headShape, ":after" );
      }

      public void OnResultExecuted(ResultExecutedContext filterContext) {}

      #endregion
   }
}