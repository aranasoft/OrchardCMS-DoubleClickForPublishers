using System.Web.Mvc;
using Arana.DoubleClickForPublishers.Models;
using Orchard;
using Orchard.ContentManagement;
using Orchard.DisplayManagement;
using Orchard.Environment.Extensions;
using Orchard.Mvc.Filters;

namespace Arana.DoubleClickForPublishers.Filters {
    [OrchardFeature("Arana.DoubleClickForPublishers")]
    public class DoubleClickForPublishersFilter : FilterProvider, IResultFilter {
        private readonly IOrchardServices _orchardServices;
        private readonly IWorkContextAccessor _workContextAccessor;

        public DoubleClickForPublishersFilter(IOrchardServices orchardServices,
                                              IShapeFactory shape,
                                              IWorkContextAccessor workContextAccessor) {
            _orchardServices = orchardServices;
            _workContextAccessor = workContextAccessor;
            Shape = shape;
        }

        public dynamic Shape { get; set; }

        public void OnResultExecuting(ResultExecutingContext filterContext) {
            var viewResult = filterContext.Result as ViewResult;
            if (viewResult == null) return;

            //Determine if we're on an admin page
            var isAdmin = Orchard.UI.Admin.AdminFilter.IsApplied(filterContext.RequestContext);

            if (isAdmin) return; // Not a valid configuration, ignore filter

            var part = _orchardServices.WorkContext.CurrentSite.As<DoubleClickForPublishersSettingsPart>();
            if (part == null) return; // Not a valid configuration, ignore filter

            var layout = _workContextAccessor.GetContext(filterContext).Layout;
            var headShape = Shape.DoubleClickForPublishersHeadScript(UseAsyncTags: part.UseAsyncTags);
            layout.Zones.Head.Add(headShape, ":after");
        }

        public void OnResultExecuted(ResultExecutedContext filterContext) {}
    }
}