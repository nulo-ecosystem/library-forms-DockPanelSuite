﻿using Nulo.Modules.DockPanelSuite.Docking;

namespace Nulo.Modules.DockPanelSuite.Themes.Default {

    internal class DefaultDockWindowFactory : DockPanelExtender.IDockWindowFactory {

        public DockWindow CreateDockWindow(DockPanel dockPanel, DockState dockState) {
            return new DefaultDockWindow(dockPanel, dockState);
        }
    }
}