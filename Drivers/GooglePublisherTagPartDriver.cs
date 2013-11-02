using Arana.DoubleClickForPublishers.Models;
using JetBrains.Annotations;
using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.Handlers;
using Orchard.Localization;

namespace Arana.DoubleClickForPublishers.Drivers {
   [UsedImplicitly]
   public class GooglePublisherTagPartDriver : ContentPartDriver<GooglePublisherTagPart> {
      private const string TemplateName = "Parts/DoubleClickForPublishers.GPT";
      private readonly IOrchardServices _services;

      public GooglePublisherTagPartDriver(IOrchardServices services) {
         _services = services;
         T = NullLocalizer.Instance;
      }

      public Localizer T { get; set; }

      protected override DriverResult Display(GooglePublisherTagPart part, string displayType,
                                              dynamic shapeHelper) {
         return ContentShape("Parts_DoubleClickForPublishers_GPT",
                             () => {
                                var publishersSettings =
                                   _services.WorkContext.CurrentSite.As<DoubleClickForPublishersSettingsPart>();
                                if (publishersSettings == null ||
                                    string.IsNullOrWhiteSpace(publishersSettings.NetworkCode)) {
                                   return null;
                                }

                                var targeting = part.Targeting;
                                return
                                   shapeHelper.Parts_DoubleClickForPublishers_GPT(
                                      Targeting: targeting,
                                      AdUnit: part.AdUnit,
                                      Height: part.Height,
                                      Width: part.Width);
                             });
      }

      protected override DriverResult Editor(GooglePublisherTagPart part, dynamic shapeHelper) {
         return ContentShape( "Parts_DoubleClickForPublishers_GPT_Edit",
                             () => shapeHelper.EditorTemplate(
                                TemplateName: TemplateName,
                                Model: part,
                                Prefix: Prefix));
      }

      //POST
      protected override DriverResult Editor(GooglePublisherTagPart part, IUpdateModel updater,
                                             dynamic shapeHelper) {
         updater.TryUpdateModel(part, Prefix, null, null);
         return Editor(part, shapeHelper);
      }

      protected override void Importing(GooglePublisherTagPart part,
                                        ImportContentContext context) {
         part.Record.AdUnit = context.Attribute(part.PartDefinition.Name, "AdUnit");
         part.Record.Targeting = context.Attribute(part.PartDefinition.Name, "Targeting");

         int height = 0;
         int.TryParse(context.Attribute(part.PartDefinition.Name, "Height"), out height);
         part.Record.Height = height;

         int width = 0;
         int.TryParse(context.Attribute(part.PartDefinition.Name, "Width"), out width);
         part.Record.Width = width;
      }

      protected override void Exporting(GooglePublisherTagPart part,
                                        ExportContentContext context) {
         context.Element(part.PartDefinition.Name).SetAttributeValue("AdUnit", part.Record.AdUnit);
         context.Element(part.PartDefinition.Name).SetAttributeValue("Targeting", part.Record.Targeting);
         context.Element(part.PartDefinition.Name).SetAttributeValue("Height", part.Record.Height);
         context.Element(part.PartDefinition.Name).SetAttributeValue("Width", part.Record.Width);
      }
   }
}