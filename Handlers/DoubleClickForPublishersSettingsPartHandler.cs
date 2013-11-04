using Arana.DoubleClickForPublishers.Models;
using JetBrains.Annotations;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Environment.Extensions;

namespace Arana.DoubleClickForPublishers.Handlers {
    [UsedImplicitly]
    [OrchardFeature("Arana.DoubleClickForPublishers")]
    public class DoubleClickForPublishersSettingsPartHandler : ContentHandler {
        public DoubleClickForPublishersSettingsPartHandler(
            IRepository<DoubleClickForPublishersSettingsPartRecord> repository) {
            Filters.Add(new ActivatingFilter<DoubleClickForPublishersSettingsPart>("Site"));
            Filters.Add(StorageFilter.For(repository));
        }
    }
}