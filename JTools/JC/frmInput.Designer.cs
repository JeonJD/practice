namespace JC
{
    partial class frmInput
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
            this.tbArgument = new System.Windows.Forms.TextBox();
            this.btInput = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tbArgument
            // 
            this.tbArgument.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbArgument.Location = new System.Drawing.Point(7, 11);
            this.tbArgument.Name = "tbArgument";
            this.tbArgument.Size = new System.Drawing.Size(162, 21);
            this.tbArgument.TabIndex = 0;
            this.tbArgument.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbArgument_KeyDown);
            // 
            // btInput
            // 
            this.btInput.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btInput.Location = new System.Drawing.Point(175, 11);
            this.btInput.Name = "btInput";
            this.btInput.Size = new System.Drawing.Size(46, 21);
            this.btInput.TabIndex = 1;
            this.btInput.Text = "확인";
            this.btInput.UseVisualStyleBackColor = true;
            this.btInput.Click += new System.EventHandler(this.btInput_Click);
            // 
            // frmInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(230, 39);
            this.Controls.Add(this.btInput);
            this.Controls.Add(this.tbArgument);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.Name = "frmInput";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmInput_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmInput_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbArgument;
        private System.Windows.Forms.Button btInput;
    }
}