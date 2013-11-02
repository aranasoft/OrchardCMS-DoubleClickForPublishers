using System;
using Orchard.ContentManagement;
using Orchard.ContentManagement.FieldStorage;

namespace Arana.DoubleClickForPublishers.Models {
   public class GooglePublisherTagField : ContentField {
      public string Targeting {
         get { return Storage.Get<string>( ); }
         set { Storage.Set( value ?? String.Empty ); }
      }
   }
}