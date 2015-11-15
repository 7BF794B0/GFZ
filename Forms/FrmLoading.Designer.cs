namespace GFZ.Forms
{
    partial class FrmLoading
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
            this.pbLoading = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // pbLoading
            // 
            this.pbLoading.Location = new System.Drawing.Point(12, 12);
            this.pbLoading.Name = "pbLoading";
            this.pbLoading.Size = new System.Drawing.Size(910, 23);
            this.pbLoading.TabIndex = 0;
            // 
            // FrmLoading
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(934, 50);
            this.Controls.Add(this.pbLoading);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximumSize = new System.Drawing.Size(952, 90);
            this.MinimumSize = new System.Drawing.Size(952, 90);
            this.Name = "FrmLoading";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Загрузка...";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar pbLoading;
    }
}