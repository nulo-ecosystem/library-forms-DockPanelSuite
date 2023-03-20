﻿using System.Collections.Generic;
using System.Windows.Forms;

namespace Nulo.Modules.DockPanelSuite.LayoutManager {

    public partial class RemoveLayoutDialog : Form {

        public int IndexLayout { get; private set; }

        public RemoveLayoutDialog(string text, string caption, string button, List<string> layouts) {
            InitializeComponent();

            LabelUserLayout.Text = text;
            RemoveButton.Text = button;
            Text = caption;

            foreach (var layout in layouts) UserLayoutOptions.Items.Add(layout);
        }

        private void RemoveButton_Click(object sender, System.EventArgs e) {
            if (UserLayoutOptions.SelectedIndex == -1) return;

            IndexLayout = UserLayoutOptions.SelectedIndex;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}