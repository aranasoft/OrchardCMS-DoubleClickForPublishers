using Orchard.ContentManagement.Records;

namespace Arana.DoubleClickForPublishers.Models {
    public class GooglePublisherTagPartRecord : ContentPartRecord {
        public virtual string AdUnit { get; set; }
        public virtual int Width { get; set; }
        public virtual int Height { get; set; }
        public virtual string Targeting { get; set; }
    }
}