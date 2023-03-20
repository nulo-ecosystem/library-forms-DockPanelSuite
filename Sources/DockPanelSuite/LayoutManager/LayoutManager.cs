using System;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using Nulo.Modules.DockPanelSuite.Docking;

namespace Nulo.Modules.DockPanelSuite.LayoutManager {

    public class LayoutManager<LayoutTheme, LayoutData> where LayoutTheme : ILayoutTheme where LayoutData : ILayoutData {

        #region Initialization
        private readonly DockPanel dock;
        private readonly ILayoutData layoutData;
        private readonly ILayoutTheme layoutTheme;

        public LayoutManager() {
            dock = new DockPanel();
            layoutTheme = Activator.CreateInstance<LayoutTheme>();
            layoutData = Activator.CreateInstance<LayoutData>();
        }
        public void Init() {
            //Definindo o estilo.
            if(layoutTheme.GetTheme(null) is ThemeBase theme) {
                SetStyle?.Invoke(new ToolStripExtender(theme));
            }

            //Definindo o layout.
            var layout = layoutData.LoadCurrentLayout();
            if(!string.IsNullOrEmpty(layout)) {
                SetLayout(layout);
            } else {
                SetLayout(layoutData.LoadDefaultLayout(null));
            }
        }
        public DockPanel GetDock() => dock;
        #endregion

        #region Theme Manager
        public delegate void ThemeHandler(ToolStripExtender style);
        public ThemeHandler SetStyle { get; set; }

        public void SetTheme(string nameTheme) {
            if(layoutTheme.GetTheme(nameTheme) is ThemeBase theme) {
                if(nameTheme is null) {
                    dock.Theme = theme;
                    SetStyle?.Invoke(new ToolStripExtender(theme));
                } else {
                    var xmlContent = dock.GenerateXml();

                    while(dock.Contents.Count != 0) {
                        var content = (DockContent)dock.Contents[0];
                        content.DockPanel = null;
                        dockContents.Add(content);
                    }
                    foreach(var window in dock.FloatWindows.ToList())
                        window.Dispose();

                    dock.Theme = theme;
                    SetStyle?.Invoke(new ToolStripExtender(theme));
                    if(currentPage != null) {
                        currentPage.SetStyle(new ToolStripExtender(theme));
                        currentPage.SetColors(theme.DockContentColorPalette);
                        currentPage.UpdateContent();
                    }

                    dock.LoadFromXml(xmlContent, GetInstanceByPersistString);

                    UpdateLayout();
                }
            }
        }
        public ThemeBase GetTheme() {
            return dock.Theme;
        }
        public bool IsDarkMode() {
            return dock.Theme is DarkTheme;
        }
        #endregion

        #region Options Manager
        private ToolStripDropDownButton dropDown;
        private ToolStripMenuItem itemNewLayout;
        private ToolStripMenuItem itemRemoveLayout;

        public ToolStripDropDownButton DropDown {
            get { return dropDown; }
            set {
                dropDown = value;

                userLayouts = layoutData.LoadAllUserLayouts();
                dockContents = new List<IDockContent>();

                itemNewLayout = new ToolStripMenuItem();
                itemNewLayout.Click += ItemNewLayout_Click;
                itemRemoveLayout = new ToolStripMenuItem();
                itemRemoveLayout.Click += ItemRemoveLayout_Click;

                dropDown.DropDownOpening += DropDown_DropDownOpening;
            }
        }

        private string itemNewLayoutText;
        public string ItemNewLayoutText {
            set { itemNewLayoutText = value; }
        }

        private string itemRemoveLayoutText;
        public string ItemRemoveLayoutText {
            set { itemRemoveLayoutText = value; }
        }

        private void DropDown_DropDownOpening(object sender, EventArgs e) {
            var dropDown = DropDownClear((ToolStripDropDownButton)sender);

            //Layouts do usuário.
            if(userLayouts.Count != 0) {
                for(int i = userLayouts.Count - 1; i > -1; i--) {
                    var item = new ToolStripMenuItem(userLayouts[i]);
                    item.Click += SelectedItem_Click;
                    dropDown.DropDownItems.Add(item);
                }
                dropDown.DropDownItems.Add(new ToolStripSeparator());
            }

            //Layouts do software
            var defaultLayouts = layoutData.LoadAllDefaultLayouts();
            if(defaultLayouts.Count != 0) {
                foreach(var layout in defaultLayouts) {
                    var item = new ToolStripMenuItem(layout.Name) { Tag = layout };
                    item.Click += SelectedItem_Click;
                    dropDown.DropDownItems.Add(item);
                }
                dropDown.DropDownItems.Add(new ToolStripSeparator());
            }

            //Novo layout
            itemNewLayout.Text = itemNewLayoutText;
            dropDown.DropDownItems.Add(itemNewLayout);

            //Apagar layout.
            if(userLayouts.Count != 0) {
                itemRemoveLayout.Text = itemRemoveLayoutText;
                dropDown.DropDownItems.Add(itemRemoveLayout);
            }
        }
        private ToolStripDropDownButton DropDownClear(ToolStripDropDownButton dropDown) {
            for(int i = 0; i < dropDown.DropDownItems.Count - 3; i++)
                if(dropDown.DropDownItems[i] is ToolStripMenuItem)
                    dropDown.DropDownItems[i].Click -= SelectedItem_Click;
            dropDown.DropDownItems.Clear();
            return dropDown;
        }
        #endregion

        #region Layout Manager
        private List<string> userLayouts;
        private List<IDockContent> dockContents;

        private void SelectedItem_Click(object sender, EventArgs e) {
            var item = (ToolStripMenuItem)sender;
            var content = item.Tag is Layout layout ? layoutData.LoadDefaultLayout(layout.Key) : layoutData.LoadUserLayout(item.Text);
            if(!string.IsNullOrEmpty(content))
                SetLayout(content);
        }

        private string newTextText;
        public string NewTextText {
            set { newTextText = value; }
        }
        private string newCaptionText;
        public string NewCaptionText {
            set { newCaptionText = value; }
        }
        private string newButtonText;
        public string NewButtonText {
            set { newButtonText = value; }
        }

        private string newExistsText;
        public string NewExistsText {
            set { newExistsText = value; }
        }
        private string newInvalidText;
        public string NewInvalidText {
            set { newInvalidText = value; }
        }

        private string warningText;
        public string WarningText {
            set { warningText = value; }
        }

        private void ItemNewLayout_Click(object sender, EventArgs e) {
            using(var dialog = new NewLayoutDialog(newTextText, newCaptionText, newButtonText, userLayouts, layoutData.LoadAllDefaultLayouts())) {
                dialog.ExistsText = newExistsText;
                dialog.InvalidText = newInvalidText;
                dialog.WarningText = warningText;
                if(dialog.ShowDialog() == DialogResult.OK && layoutData.SaveUserLayout(dialog.LayoutName, dock.GenerateXml())) {
                    userLayouts.Add(dialog.LayoutName);
                }
            }
        }

        private string removeTextText;
        public string RemoveTextText {
            set { removeTextText = value; }
        }

        private string removeCaptionText;
        public string RemoveCaptionText {
            set { removeCaptionText = value; }
        }

        private string removeButtonText;
        public string RemoveButtonText {
            set { removeButtonText = value; }
        }

        private void ItemRemoveLayout_Click(object sender, EventArgs e) {
            using(var dialog = new RemoveLayoutDialog(removeTextText, removeCaptionText, removeButtonText, userLayouts)) {
                if(dialog.ShowDialog() == DialogResult.OK && layoutData.RemoveUserLayout(userLayouts[dialog.IndexLayout])) {
                    userLayouts.RemoveAt(dialog.IndexLayout);
                }
            }
        }

        private void SetLayout(string xmlContent) {
            while(dock.Contents.Count != 0) {
                var content = (DockContent)dock.Contents[0];
                content.DockPanel = null;
                dockContents.Add(content);
            }

            dock.LoadFromXml(xmlContent, GetInstanceByPersistString);

            while(dockContents.Count != 0) {
                ((DockContent)dockContents[0]).Close();
                dockContents.RemoveAt(0);
            }
        }
        public IDockContent GetInstanceByPersistString(string persistString) {
            for(int i = 0; i < dockContents.Count; i++) {
                var content = dockContents[i];
                if(content.GetType().FullName.Equals(persistString)) {
                    dockContents.RemoveAt(i);
                    return content;
                }
            }

            var panel = layoutData.GetInstanceByPersistString(persistString);
            panel.UpdateContent();
            return panel;
        }

        public void UpdateLayout() {
            foreach(var panel in dock.Contents) {
                panel.UpdateContent();
            }
        }
        public void FinishContent() {
            foreach(var panel in dock.Contents) {
                panel.FinishContent();
            }
        }
        #endregion

        #region Panels Manager
        private DockContent currentPage;
        public void OpenPage<T>() where T : DockContent {
            using(var page = Activator.CreateInstance<T>()) {
                page.SetColors(dock.Theme.DockContentColorPalette);
                page.SetStyle(new ToolStripExtender(dock.Theme));
                page.Update();
                currentPage = page;
                page.ShowDialog();
            }
        }
        public T OpenDialog<T>() where T : DockContent {
            var dialog = Activator.CreateInstance<T>();
            dialog.SetColors(dock.Theme.DockContentColorPalette);
            return dialog;
        }
        public T OpenPanel<T>() where T : DockContent {
            if(GetPanelByType<T>() is T panel) {
                panel.Activate();
                panel.UpdateContent();
            } else {
                panel = Activator.CreateInstance<T>();
                panel.Name = typeof(T).Name;
                panel.SetColors(dock.Theme.DockContentColorPalette);
                panel.SetStyle(new ToolStripExtender(dock.Theme));
                panel.UpdateContent();
                panel.Show(dock);
            }

            return panel;
        }

        public DockContent GetPanelByType<T>() where T : DockContent {
            foreach(DockContent content in dock.Contents) {
                if(content.Name.Equals(typeof(T).Name)) {
                    return content;
                }
            }
            return null;
        }
        public DockContent GetCurrentPage() {
            return currentPage;
        }
        #endregion

        public void Dispose() {
            layoutData.SaveCurrentLayout(dock.GenerateXml());
            GC.SuppressFinalize(this);
        }
    }
}