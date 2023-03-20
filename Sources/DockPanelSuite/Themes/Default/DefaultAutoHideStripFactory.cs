using Nulo.Modules.DockPanelSuite.Docking;

namespace Nulo.Modules.DockPanelSuite.Themes.Default {

    internal class DefaultAutoHideStripFactory : DockPanelExtender.IAutoHideStripFactory {

        public AutoHideStripBase CreateAutoHideStrip(DockPanel panel) {
            return new DefaultAutoHideStrip(panel);
        }
    }
}