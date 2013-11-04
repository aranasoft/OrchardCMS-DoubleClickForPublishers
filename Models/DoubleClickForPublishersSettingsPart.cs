using Orchard.ContentManagement;

namespace Arana.DoubleClickForPublishers.Models {
    public class DoubleClickForPublishersSettingsPart : ContentPart<DoubleClickForPublishersSettingsPartRecord> {
        public bool UseAsyncTags {
            get { return Record.UseAsyncTags; }
            set { Record.UseAsyncTags = value; }
        }

        public string NetworkCode {
            get { return Record.NetworkCode; }
            set { Record.NetworkCode = value; }
        }
    }
}