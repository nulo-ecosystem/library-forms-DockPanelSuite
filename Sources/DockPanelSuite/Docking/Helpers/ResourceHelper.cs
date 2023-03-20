using System.Resources;

namespace Nulo.Modules.DockPanelSuite.Docking {

    internal static class ResourceHelper {

        private static ResourceManager _resourceManager = null;

        private static ResourceManager ResourceManager {
            get {
                if (_resourceManager == null)
                    _resourceManager = new ResourceManager("Nulo.Modules.DockPanelSuite.Docking.Strings", typeof(ResourceHelper).Assembly);
                return _resourceManager;
            }

        }

        public static string GetString(string name) {
            return ResourceManager.GetString(name);
        }
    }
}