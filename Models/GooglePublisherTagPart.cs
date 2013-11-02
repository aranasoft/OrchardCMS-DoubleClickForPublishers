using Orchard.ContentManagement;

namespace Arana.DoubleClickForPublishers.Models {
   public class GooglePublisherTagPart : ContentPart<GooglePublisherTagPartRecord> {
      public virtual string Targeting {
         get { return Record.Targeting; }
         set { Record.Targeting = value; }
      }

      public virtual string AdUnit {
         get { return Record.AdUnit; }
         set { Record.AdUnit = value; }
      }

      public virtual int Width {
         get { return Record.Width; }
         set { Record.Width = value; }
      }

      public virtual int Height {
         get { return Record.Height; }
         set { Record.Height = value; }
      }
   }
}