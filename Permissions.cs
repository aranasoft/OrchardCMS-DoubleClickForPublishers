using System.Collections.Generic;
using Orchard.Environment.Extensions.Models;
using Orchard.Security.Permissions;

namespace Arana.DoubleClickForPublishers {
   public class Permissions : IPermissionProvider {
      public static readonly Permission ConfigureDoubleClickForPublishers = new Permission {
         Description = "Configure DoubleClick for Publishers",
         Name = "ConfigureDoubleClickForPublishers"
      };

      public virtual Feature Feature { get; set; }

      public IEnumerable<Permission> GetPermissions() {
         return new[] {ConfigureDoubleClickForPublishers};
      }

      public IEnumerable<PermissionStereotype> GetDefaultStereotypes() {
         return new[]
         {new PermissionStereotype {Name = "Administrator", Permissions = new[] {ConfigureDoubleClickForPublishers}}};
      }
   }
}