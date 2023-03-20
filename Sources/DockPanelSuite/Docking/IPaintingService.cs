using System.Drawing;


namespace Nulo.Modules.DockPanelSuite.Docking {

    public interface IPaintingService {
        Pen GetPen(Color color, int thickness = 1);
        SolidBrush GetBrush(Color color);
        void CleanUp();
    }
}