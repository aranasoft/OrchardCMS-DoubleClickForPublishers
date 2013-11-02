using Orchard.ContentManagement.Records;

namespace Arana.DoubleClickForPublishers.Models {
   public class DoubleClickForPublishersSettingsPartRecord : ContentPartRecord {
      public DoubleClickForPublishersSettingsPartRecord() {
         UseAsyncTags = true;
      }
      public virtual bool UseAsyncTags { get; set; }
      public virtual string NetworkCode { get; set; }
   }
}