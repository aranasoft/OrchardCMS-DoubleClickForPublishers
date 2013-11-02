using System.Collections.Generic;
using Orchard;

namespace Arana.DoubleClickForPublishers.Services {
   public interface IAdService : IUnitOfWorkDependency {
      IEnumerable<string> GetZones();
      string BuildTargetingCommand(string targetList);
      IList<AdServiceQueueEntry> Queue { get; }
   }
}