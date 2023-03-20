using Nulo.Modules.DockPanelSuite.Docking;

namespace Nulo.Modules.DockPanelSuite.Themes.Default {

    internal class DefaultWindowSplitterControlFactory : DockPanelExtender.IWindowSplitterControlFactory {

        public SplitterBase CreateSplitterControl(ISplitterHost host) {
            return new DefaultWindowSplitterControl(host);
        }
    }
}