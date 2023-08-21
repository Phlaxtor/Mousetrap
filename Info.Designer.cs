namespace Mousetrap
{
    partial class Info
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            _layout = new TableLayoutPanel();
            _text = new TextBox();
            _ok = new Button();
            _layout.SuspendLayout();
            SuspendLayout();
            // 
            // _layout
            // 
            _layout.ColumnCount = 1;
            _layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _layout.Controls.Add(_text, 0, 0);
            _layout.Controls.Add(_ok, 0, 1);
            _layout.Dock = DockStyle.Fill;
            _layout.Location = new Point(0, 0);
            _layout.Name = "_layout";
            _layout.RowCount = 2;
            _layout.RowStyles.Add(new RowStyle(SizeType.Percent, 70.03745F));
            _layout.RowStyles.Add(new RowStyle(SizeType.Percent, 29.9625473F));
            _layout.Size = new Size(184, 111);
            _layout.TabIndex = 0;
            // 
            // _text
            // 
            _text.Dock = DockStyle.Fill;
            _text.Enabled = false;
            _text.Location = new Point(3, 3);
            _text.Multiline = true;
            _text.Name = "_text";
            _text.Size = new Size(178, 71);
            _text.TabIndex = 0;
            // 
            // _ok
            // 
            _ok.Dock = DockStyle.Fill;
            _ok.Location = new Point(3, 80);
            _ok.Name = "_ok";
            _ok.Size = new Size(178, 28);
            _ok.TabIndex = 1;
            _ok.Text = "Ok";
            _ok.UseVisualStyleBackColor = true;
            // 
            // Info
            // 
            AcceptButton = _ok;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = _ok;
            ClientSize = new Size(184, 111);
            ControlBox = false;
            Controls.Add(_layout);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Info";
            ShowIcon = false;
            ShowInTaskbar = false;
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Message";
            TopMost = true;
            _layout.ResumeLayout(false);
            _layout.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel _layout;
        private TextBox _text;
        private Button _ok;
    }
}