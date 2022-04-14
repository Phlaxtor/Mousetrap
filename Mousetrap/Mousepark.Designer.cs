namespace Mousetrap
{
    partial class Mousepark
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.parkLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // parkLabel
            // 
            this.parkLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.parkLabel.AutoSize = true;
            this.parkLabel.Location = new System.Drawing.Point(0, 0);
            this.parkLabel.Margin = new System.Windows.Forms.Padding(0);
            this.parkLabel.MinimumSize = new System.Drawing.Size(210, 45);
            this.parkLabel.Name = "parkLabel";
            this.parkLabel.Size = new System.Drawing.Size(210, 45);
            this.parkLabel.TabIndex = 0;
            this.parkLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Mousepark
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(208, 44);
            this.Controls.Add(this.parkLabel);
            this.MaximumSize = new System.Drawing.Size(230, 100);
            this.MinimumSize = new System.Drawing.Size(230, 100);
            this.Name = "Mousepark";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label parkLabel;
    }
}