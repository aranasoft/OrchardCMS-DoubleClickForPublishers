using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Orchard.Environment.Extensions;
using Orchard.Themes.Services;

namespace Arana.DoubleClickForPublishers.Services {
    public class AdServiceQueueEntry {
        public string AdUnit { get; set; }
        public string AdSize { get; set; }
        public string AdElement { get; set; }
        public string AdTargeting { get; set; }
    }

    [UsedImplicitly]
    public class AdService : IAdService {
        private readonly ISiteThemeService _siteThemeService;
        private readonly IExtensionManager _extensionManager;

        public AdService(
            ISiteThemeService siteThemeService,
            IExtensionManager extensionManager) {
            _siteThemeService = siteThemeService;
            _extensionManager = extensionManager;
        }

        private IList<AdServiceQueueEntry> _queue;

        public IList<AdServiceQueueEntry> Queue {
            get { return _queue ?? (_queue = new List<AdServiceQueueEntry>()); }
        }

        public IEnumerable<string> GetZones() {
            var theme = _siteThemeService.GetSiteTheme();
            IEnumerable<string> zones = new List<string>();

            // get the zones for this theme
            if (theme.Zones != null) {
                zones = theme.Zones.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim())
                    .Distinct()
                    .ToList();
            }

            // if this theme has no zones defined then walk the BaseTheme chain until we hit a theme which defines zones
            while (!zones.Any() && theme != null && !string.IsNullOrWhiteSpace(theme.BaseTheme)) {
                var baseTheme = theme.BaseTheme;
                theme = _extensionManager.GetExtension(baseTheme);
                if (theme != null && theme.Zones != null) {
                    zones = theme.Zones.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => x.Trim())
                        .Distinct()
                        .ToList();
                }
            }

            return zones;
        }

        public string BuildTargetingCommand(string targeting) {
            var targets = ParseTarget(targeting);
            var targetScript = new StringBuilder();
            foreach (var target in targets) {
                targetScript.AppendFormat(".setTargeting(\"{0}\",{1})",
                    target.Key,
                    JsonConvert.SerializeObject(target.Value));
            }
            return targetScript.ToString();
        }

        protected Dictionary<string, object> ParseTarget(string tagTarget) {
            var targetList = tagTarget ?? string.Empty;
            var targetPairs = targetList.Split(';').Select(val => val.Trim()).Where(val => val.Contains("="));
            var targets = targetPairs.Select(val => val.Split('='))
                // Both sides of the pair are not empty
                .Where(pair => !string.IsNullOrWhiteSpace(pair[0]) && !string.IsNullOrWhiteSpace(pair[1]))
                .ToDictionary(val => val[0].Trim(), val => GetParseTargetingValue(val[1]));
            return targets;
        }

        protected object GetParseTargetingValue(string value) {
            var targetingArray = value.Split(',').Select(val => val.Trim()).ToList();
            return targetingArray.Count() > 1 ? (object) targetingArray : targetingArray.FirstOrDefault();
        }
    }
}