using Arana.DoubleClickForPublishers.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Localization;

namespace Arana.DoubleClickForPublishers.Drivers {
   public class DoubleClickForPublishersSettingsPartDriver : ContentPartDriver<DoubleClickForPublishersSettingsPart> {
      private const string TemplateName = "Parts/DoubleClickForPublishersSettings";

      public DoubleClickForPublishersSettingsPartDriver( ) {
         T = NullLocalizer.Instance;
      }

      public Localizer T { get; set; }

      protected override string Prefix {
         get { return "DoubleClickForPublishersSettings"; }
      }

      //GET
      protected override DriverResult Editor(DoubleClickForPublishersSettingsPart part, dynamic shapeHelper) {
         return ContentShape("Parts_DoubleClickForPublishersSettings_Edit",
                             () => shapeHelper.EditorTemplate(
                                TemplateName: TemplateName,
                                Model: part,
                                Prefix: Prefix));
      }

      //POST
      protected override DriverResult Editor(DoubleClickForPublishersSettingsPart part, IUpdateModel updater,
                                             dynamic shapeHelper) {
         updater.TryUpdateModel(part, Prefix, null, null);
         return Editor(part, shapeHelper);
      }
   }
}