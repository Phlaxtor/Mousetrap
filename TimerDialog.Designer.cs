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
            this._layout = new System.Windows.Forms.TableLayoutPanel();
            this._ok = new System.Windows.Forms.Button();
            this._hours = new System.Windows.Forms.NumericUpDown();
            this._minutes = new System.Windows.Forms.NumericUpDown();
            this._seconds = new System.Windows.Forms.NumericUpDown();
            this._layout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._hours)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._minutes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._seconds)).BeginInit();
            this.SuspendLayout();
            // 
            // _layout
            // 
            this._layout.ColumnCount = 3;
            this._layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this._layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this._layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this._layout.Controls.Add(this._ok, 1, 1);
            this._layout.Controls.Add(this._hours, 0, 0);
            this._layout.Controls.Add(this._minutes, 1, 0);
            this._layout.Controls.Add(this._seconds, 2, 0);
            this._layout.Dock = System.Windows.Forms.DockStyle.Fill;
            this._layout.Location = new System.Drawing.Point(0, 0);
            this._layout.Margin = new System.Windows.Forms.Padding(2);
            this._layout.Name = "_layout";
            this._layout.RowCount = 2;
            this._layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 12F));
            this._layout.Size = new System.Drawing.Size(125, 50);
            this._layout.TabIndex = 4;
            // 
            // _ok
            // 
            this._layout.SetColumnSpan(this._ok, 2);
            this._ok.Dock = System.Windows.Forms.DockStyle.Fill;
            this._ok.Location = new System.Drawing.Point(43, 27);
            this._ok.Margin = new System.Windows.Forms.Padding(2);
            this._ok.Name = "_ok";
            this._ok.Size = new System.Drawing.Size(80, 21);
            this._ok.TabIndex = 3;
            this._ok.Text = "Ok";
            this._ok.UseVisualStyleBackColor = true;
            // 
            // _hours
            // 
            this._hours.Dock = System.Windows.Forms.DockStyle.Fill;
            this._hours.Location = new System.Drawing.Point(2, 2);
            this._hours.Margin = new System.Windows.Forms.Padding(2);
            this._hours.Maximum = new decimal(new int[] {
            23,
            0,
            0,
            0});
            this._hours.Name = "_hours";
            this._hours.Size = new System.Drawing.Size(37, 23);
            this._hours.TabIndex = 0;
            // 
            // _minutes
            // 
            this._minutes.Dock = System.Windows.Forms.DockStyle.Fill;
            this._minutes.Location = new System.Drawing.Point(43, 2);
            this._minutes.Margin = new System.Windows.Forms.Padding(2);
            this._minutes.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
            this._minutes.Name = "_minutes";
            this._minutes.Size = new System.Drawing.Size(37, 23);
            this._minutes.TabIndex = 1;
            // 
            // _seconds
            // 
            this._seconds.Dock = System.Windows.Forms.DockStyle.Fill;
            this._seconds.Location = new System.Drawing.Point(84, 2);
            this._seconds.Margin = new System.Windows.Forms.Padding(2);
            this._seconds.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
            this._seconds.Name = "_seconds";
            this._seconds.Size = new System.Drawing.Size(39, 23);
            this._seconds.TabIndex = 2;
            // 
            // TimerDialog
            // 
            this.AcceptButton = this._ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(125, 50);
            this.Controls.Add(this._layout);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TimerDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Timer";
            this.TopMost = true;
            this._layout.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._hours)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._minutes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._seconds)).EndInit();
            this.ResumeLayout(false);

    }

    #endregion
    private TableLayoutPanel _layout;
    private Button _ok;
    private NumericUpDown _hours;
    private NumericUpDown _minutes;
    private NumericUpDown _seconds;
}