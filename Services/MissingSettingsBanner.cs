using System.Collections.Generic;
using Arana.DoubleClickForPublishers.Models;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.UI.Admin.Notification;
using Orchard.UI.Notify;

namespace Arana.DoubleClickForPublishers.Services {
    [OrchardFeature("Arana.DoubleClickForPublishers")]
    public class MissingSettingsBanner : INotificationProvider {
        private readonly IOrchardServices _orchardServices;

        public MissingSettingsBanner(IOrchardServices orchardServices) {
            _orchardServices = orchardServices;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public IEnumerable<NotifyEntry> GetNotifications() {
            var doubleClickForPublishersSettings =
                _orchardServices.WorkContext.CurrentSite.As<DoubleClickForPublishersSettingsPart>();
            if (doubleClickForPublishersSettings == null ||
                string.IsNullOrWhiteSpace(doubleClickForPublishersSettings.NetworkCode)) {
                yield return
                    new NotifyEntry {
                        Message = T("The Double Click for Publishers settings need to be configured."),
                        Type = NotifyType.Warning
                    };
            }
        }
    }
}