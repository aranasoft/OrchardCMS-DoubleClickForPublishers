using Arana.DoubleClickForPublishers.Models;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;

namespace Arana.DoubleClickForPublishers {
    public class Migrations : DataMigrationImpl {
        public int Create() {
            SchemaBuilder
                .CreateTable(typeof (DoubleClickForPublishersSettingsPartRecord).Name,
                    table => table.ContentPartRecord()
                        .Column<string>("NetworkCode")
                        .Column<bool>("UseAsyncTags")
                );

            SchemaBuilder
                .CreateTable(typeof (GooglePublisherTagPartRecord).Name,
                    table => table.ContentPartRecord()
                        .Column<string>("Targeting")
                        .Column<string>("AdUnit")
                        .Column<int>("Width")
                        .Column<int>("Height")
                );

            ContentDefinitionManager
                .AlterPartDefinition("GooglePublisherTagPart",
                    cfg => cfg.Attachable(false)
                        .WithDescription("Displays a Google Publisher Tag within a Content Item")
                );

            ContentDefinitionManager
                .AlterTypeDefinition("GooglePublisherTagWidget",
                    cfg => cfg.Creatable(false)
                        .WithPart("CommonPart")
                        .WithPart("IdentityPart")
                        .WithPart("WidgetPart")
                        .WithPart("GooglePublisherTagPart")
                        .WithSetting("Stereotype", "Widget")
                );

            return 1;
        }
    }
}