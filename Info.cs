namespace Mousetrap
{
    public partial class Info : Form
    {
        public Info()
        {
            InitializeComponent();
            _ok.Click += Ok;
        }

        public void Clear()
        {
            Visible = false;
            _text.Text = string.Empty;
        }

        public void SetMessage(string message)
        {
            if (string.IsNullOrEmpty(message)) return;
            _text.Text = message;
            Visible = true;
            Refresh();
        }

        private void Ok(object? sender, EventArgs e)
        {
            Clear();
        }
    }
}