using System.Globalization;
using Arana.DoubleClickForPublishers.Models;
using Arana.DoubleClickForPublishers.Settings;
using JetBrains.Annotations;
using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.Handlers;
using Orchard.DisplayManagement.Shapes;
using Orchard.Localization;

namespace Arana.DoubleClickForPublishers.Drivers {
    [UsedImplicitly]
    public class GooglePublisherTagFieldDriver : ContentFieldDriver<GooglePublisherTagField> {
        private readonly IOrchardServices _services;
        private readonly IWorkContextAccessor _workContextAccessor;

        private const string TemplateName = "Fields/DoubleClickForPublishers.GPT";

        public GooglePublisherTagFieldDriver(IOrchardServices services,
                                             IWorkContextAccessor workContextAccessor) {
            _services = services;
            _workContextAccessor = workContextAccessor;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        private static string GetPrefix(ContentField field, ContentPart part) {
            // handles spaces in field names
            return (part.PartDefinition.Name + "." + field.Name)
                .Replace(" ", "_");
        }

        protected override DriverResult Display(ContentPart part,
                                                GooglePublisherTagField field,
                                                string displayType,
                                                dynamic shapeHelper) {
            return ContentShape("Fields_DoubleClickForPublishers_GPT",
                field.Name,
                () => {
                    var publishersSettings =
                        _services.WorkContext.CurrentSite.As<DoubleClickForPublishersSettingsPart>();
                    if (publishersSettings == null ||
                        string.IsNullOrWhiteSpace(publishersSettings.NetworkCode)) return null;

                    var fieldSettings = field.PartFieldDefinition.Settings
                        .GetModel<GooglePublisherTagPartFieldSettings>();

                    var targeting = string.Format("{0};{1}", fieldSettings.Targeting, field.Targeting);
                    var shape =
                        shapeHelper.Fields_DoubleClickForPublishers_GPT(
                            Targeting: targeting,
                            AdUnit: fieldSettings.AdUnit,
                            Height: fieldSettings.Height,
                            Width: fieldSettings.Width);
                    var zone = GetThemeZone(field);

                    if (!string.IsNullOrWhiteSpace(zone)) {
                        var position = GetPosition(field);
                        var zoneSection = _workContextAccessor.GetContext().Layout.Zones[zone];
                        if (position > 0) zoneSection.Add(shape, position.ToString(CultureInfo.InvariantCulture));
                        else zoneSection.Add(shape);
                        return new Shape();
                    }

                    return shape;
                });
        }

        protected override DriverResult Editor(ContentPart part,
                                               GooglePublisherTagField field,
                                               dynamic shapeHelper) {
            return ContentShape("Fields_DoubleClickForPublishers_GPT_Edit",
                () => shapeHelper.EditorTemplate(
                    TemplateName: TemplateName,
                    Model: field,
                    Prefix: GetPrefix(field, part)));
        }

        //POST
        protected override DriverResult Editor(ContentPart part,
                                               GooglePublisherTagField field,
                                               IUpdateModel updater,
                                               dynamic shapeHelper) {
            updater.TryUpdateModel(field, GetPrefix(field, part), null, null);
            return Editor(part, field, shapeHelper);
        }

        private static string GetThemeZone(GooglePublisherTagField part) {
            var typePartSettings = part.PartFieldDefinition.Settings.GetModel<GooglePublisherTagPartFieldSettings>();
            return (typePartSettings != null && !string.IsNullOrWhiteSpace(typePartSettings.ThemeZone))
                ? typePartSettings.ThemeZone
                : null;
        }

        private static int GetPosition(GooglePublisherTagField part) {
            var typePartSettings = part.PartFieldDefinition.Settings.GetModel<GooglePublisherTagPartFieldSettings>();
            return (typePartSettings != null)
                ? typePartSettings.Position
                : 0;
        }

        protected override void Importing(ContentPart part,
                                          GooglePublisherTagField field,
                                          ImportContentContext context) {
            context.ImportAttribute(GetPrefix(field, part), "Targeting", val => field.Targeting = val);
        }

        protected override void Exporting(ContentPart part,
                                          GooglePublisherTagField field,
                                          ExportContentContext context) {
            context.Element(GetPrefix(field, part)).SetAttributeValue("Targeting", field.Targeting);
        }
    }
}