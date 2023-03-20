using Nulo.Modules.DockPanelSuite.Docking;

namespace Nulo.Modules.DockPanelSuite.Themes.Default {

    public class DefaultDockPaneStripFactory : DockPanelExtender.IDockPaneStripFactory {

        public DockPaneStripBase CreateDockPaneStrip(DockPane pane) {
            return new DefaultDockPaneStrip(pane);
        }
    }
}