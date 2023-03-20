using Nulo.Modules.DockPanelSuite.Docking;

namespace Nulo.Modules.DockPanelSuite.Themes.Default {

    internal class DefaultDockPaneSplitterControlFactory : DockPanelExtender.IDockPaneSplitterControlFactory {

        public DockPane.SplitterControlBase CreateSplitterControl(DockPane pane) {
            return new DefaultSplitterControl(pane);
        }
    }
}