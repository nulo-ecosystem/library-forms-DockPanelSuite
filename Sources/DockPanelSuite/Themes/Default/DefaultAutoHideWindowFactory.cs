using Nulo.Modules.DockPanelSuite.Docking;

namespace Nulo.Modules.DockPanelSuite.Themes.Default {

    internal class DefaultAutoHideWindowFactory : DockPanelExtender.IAutoHideWindowFactory {

        public DockPanel.AutoHideWindowControl CreateAutoHideWindow(DockPanel panel) {
            return new DefaultAutoHideWindowControl(panel);
        }
    }
}