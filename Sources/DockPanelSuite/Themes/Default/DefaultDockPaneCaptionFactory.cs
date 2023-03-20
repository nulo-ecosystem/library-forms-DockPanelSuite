using Nulo.Modules.DockPanelSuite.Docking;

namespace Nulo.Modules.DockPanelSuite.Themes.Default {

    internal class DefaultDockPaneCaptionFactory : DockPanelExtender.IDockPaneCaptionFactory {

        public DockPaneCaptionBase CreateDockPaneCaption(DockPane pane) {
            return new DefaultDockPaneCaption(pane);
        }
    }
}