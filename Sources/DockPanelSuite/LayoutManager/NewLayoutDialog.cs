using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Nulo.Modules.DockPanelSuite.LayoutManager {

    public partial class NewLayoutDialog : Form {

        public string LayoutName { get; private set; }
        private const string BADCHARS = "\\/:*?\"<>|";
        private readonly List<Layout> defaultLayouts;
        private readonly List<string> userLayouts;

        public NewLayoutDialog(string text, string caption, string button, List<string> userLayouts, List<Layout> defaultLayouts) {
            InitializeComponent();

            AddButton.Text = button;
            LabelLayoutName.Text = text;
            Text = caption;

            this.userLayouts = userLayouts;
            this.defaultLayouts = defaultLayouts;
        }

        private string existsText;
        public string ExistsText {
            set { existsText = value; }
        }

        private string invalidText;
        public string InvalidText {
            set { invalidText = value; }
        }

        private string warningText;
        public string WarningText {
            set { warningText = value; }
        }

        private void SetName() {
            var value = TextLayoutName.Text.Trim();
            if (IsValidFileName(value)) {
                LayoutName = value;
                DialogResult = DialogResult.OK;
                Close();
            }
        }
        private bool IsValidFileName(string name) {
            if (string.IsNullOrEmpty(name))
                return false;

            if (userLayouts.Contains(name) || defaultLayouts.FirstOrDefault(a => a.Name.Equals(name)) != null) {
                MessageBoxManager.Show(existsText, warningText, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            foreach (var badChar in BADCHARS) {
                if (name.IndexOf(badChar) != -1) {
                    MessageBoxManager.Show($"{invalidText} {BADCHARS}", warningText, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }

            return true;
        }

        private void AddButton_Click(object sender, System.EventArgs e) => SetName();
        private void TextLayoutName_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter)
                SetName();
        }
    }
}
