namespace Mousetrap;

partial class TimerDialog
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
        _ok = new Button();
        _hours = new NumericUpDown();
        _minutes = new NumericUpDown();
        _seconds = new NumericUpDown();
        _layout.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)_hours).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_minutes).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_seconds).BeginInit();
        SuspendLayout();
        // 
        // _layout
        // 
        _layout.ColumnCount = 3;
        _layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33333F));
        _layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33333F));
        _layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33333F));
        _layout.Controls.Add(_ok, 1, 1);
        _layout.Controls.Add(_hours, 0, 0);
        _layout.Controls.Add(_minutes, 1, 0);
        _layout.Controls.Add(_seconds, 2, 0);
        _layout.Dock = DockStyle.Fill;
        _layout.Location = new Point(0, 0);
        _layout.Margin = new Padding(2);
        _layout.Name = "_layout";
        _layout.RowCount = 2;
        _layout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        _layout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        _layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 12F));
        _layout.Size = new Size(125, 50);
        _layout.TabIndex = 4;
        // 
        // _ok
        // 
        _layout.SetColumnSpan(_ok, 2);
        _ok.Dock = DockStyle.Fill;
        _ok.Location = new Point(43, 27);
        _ok.Margin = new Padding(2);
        _ok.Name = "_ok";
        _ok.Size = new Size(80, 21);
        _ok.TabIndex = 3;
        _ok.Text = "Ok";
        _ok.UseVisualStyleBackColor = true;
        // 
        // _hours
        // 
        _hours.Dock = DockStyle.Fill;
        _hours.Location = new Point(2, 2);
        _hours.Margin = new Padding(2);
        _hours.Maximum = new decimal(new int[] { 23, 0, 0, 0 });
        _hours.Name = "_hours";
        _hours.Size = new Size(37, 23);
        _hours.TabIndex = 0;
        // 
        // _minutes
        // 
        _minutes.Dock = DockStyle.Fill;
        _minutes.Location = new Point(43, 2);
        _minutes.Margin = new Padding(2);
        _minutes.Maximum = new decimal(new int[] { 59, 0, 0, 0 });
        _minutes.Name = "_minutes";
        _minutes.Size = new Size(37, 23);
        _minutes.TabIndex = 1;
        // 
        // _seconds
        // 
        _seconds.Dock = DockStyle.Fill;
        _seconds.Location = new Point(84, 2);
        _seconds.Margin = new Padding(2);
        _seconds.Maximum = new decimal(new int[] { 59, 0, 0, 0 });
        _seconds.Name = "_seconds";
        _seconds.Size = new Size(39, 23);
        _seconds.TabIndex = 2;
        // 
        // TimerDialog
        // 
        AcceptButton = _ok;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(125, 50);
        Controls.Add(_layout);
        FormBorderStyle = FormBorderStyle.FixedToolWindow;
        Margin = new Padding(2);
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "TimerDialog";
        ShowIcon = false;
        ShowInTaskbar = false;
        SizeGripStyle = SizeGripStyle.Hide;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Timer";
        TopMost = true;
        _layout.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)_hours).EndInit();
        ((System.ComponentModel.ISupportInitialize)_minutes).EndInit();
        ((System.ComponentModel.ISupportInitialize)_seconds).EndInit();
        ResumeLayout(false);
    }

    #endregion
    private TableLayoutPanel _layout;
    private Button _ok;
    private NumericUpDown _hours;
    private NumericUpDown _minutes;
    private NumericUpDown _seconds;
}