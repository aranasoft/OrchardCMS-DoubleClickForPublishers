using System.Collections.Generic;
using System.Globalization;
using Arana.DoubleClickForPublishers.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Builders;
using Orchard.ContentManagement.MetaData.Models;
using Orchard.ContentManagement.ViewModels;

namespace Arana.DoubleClickForPublishers.Settings {
   public class GooglePublisherTagSettingsHooks : ContentDefinitionEditorEventsBase {
      private string _fieldTypeName;
      private string _fieldSettingsName;

      public string FieldTypeName {
         get { return _fieldTypeName ?? (_fieldTypeName = typeof (GooglePublisherTagField).Name); }
         set { _fieldTypeName = value; }
      }

      public string FieldSettingsName {
         get { return _fieldSettingsName ?? (_fieldSettingsName = typeof (GooglePublisherTagPartFieldSettings).Name); }
         set { _fieldSettingsName = value; }
      }

      public override IEnumerable<TemplateViewModel> PartFieldEditor(ContentPartFieldDefinition definition) {
         if (definition.FieldDefinition.Name != FieldTypeName) {
            yield break;
         }

         var model = definition.Settings.GetModel<GooglePublisherTagPartFieldSettings>();

         yield return DefinitionTemplate(model);
      }

      public override IEnumerable<TemplateViewModel> PartFieldEditorUpdate(ContentPartFieldDefinitionBuilder builder,
                                                                           IUpdateModel updateModel) {
         if (builder.FieldType != FieldTypeName) {
            yield break;
         }

         var model = new GooglePublisherTagPartFieldSettings();
         updateModel.TryUpdateModel(model, FieldSettingsName, null, null);
         builder.WithSetting(string.Format("{0}.AdUnit", FieldSettingsName),
                             !string.IsNullOrWhiteSpace(model.AdUnit) ? model.AdUnit : null)
                .WithSetting(string.Format("{0}.ThemeZone", FieldSettingsName),
                             !string.IsNullOrWhiteSpace(model.ThemeZone) ? model.ThemeZone : null)
                .WithSetting(string.Format("{0}.Position", FieldSettingsName),
                             model.Position.ToString(CultureInfo.InvariantCulture))
                .WithSetting(string.Format("{0}.Height", FieldSettingsName),
                             model.Height.ToString(CultureInfo.InvariantCulture))
                .WithSetting(string.Format("{0}.Width", FieldSettingsName),
                             model.Width.ToString(CultureInfo.InvariantCulture));
         yield return DefinitionTemplate(model);
      }
   }
}