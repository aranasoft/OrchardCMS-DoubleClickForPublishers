using Orchard.UI.Resources;

namespace Arana.DoubleClickForPublishers {
    public class ResourceManifest : IResourceManifestProvider {
        public void BuildManifests(ResourceManifestBuilder builder) {
           builder.Add( ).DefineStyle( "GooglePublisherTags" ).SetUrl( "GooglePublisherTags.css" );
        }
    }
}
