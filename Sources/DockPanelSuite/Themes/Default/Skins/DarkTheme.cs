namespace Nulo.Modules.DockPanelSuite.Docking {

    using Themes.Default;

    public class DarkTheme : Theme {

        public DarkTheme() : base(Decompress(Resources.default_dark_theme)) {
            DockContentColorPalette = new DockContentColorPalette {
                Background = System.Drawing.Color.FromArgb(37, 37, 37),
                DarkBackground = System.Drawing.Color.FromArgb(30, 30, 30),
                LightBackground = System.Drawing.Color.FromArgb(42, 42, 42),
                UnfocusedBackColor = System.Drawing.Color.Gray,
                TextColor = System.Drawing.Color.White,
                Control = ColorPalette.CommandBarMenuDefault.Background
            };
        }
    }
}