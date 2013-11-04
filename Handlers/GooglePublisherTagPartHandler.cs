using Arana.DoubleClickForPublishers.Models;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;

namespace Arana.DoubleClickForPublishers.Handlers {
    public class GooglePublisherTagPartHandler : ContentHandler {
        public GooglePublisherTagPartHandler(
            IRepository<GooglePublisherTagPartRecord> repository) {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}