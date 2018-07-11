namespace JC
{
    partial class frmPreset
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
            this.cbToggle = new System.Windows.Forms.CheckBox();
            this.cbArgument = new System.Windows.Forms.CheckBox();
            this.lbCommand_1 = new System.Windows.Forms.Label();
            this.tbCommand_1 = new System.Windows.Forms.TextBox();
            this.lbCommand_2 = new System.Windows.Forms.Label();
            this.tbCommand_2 = new System.Windows.Forms.TextBox();
            this.btSubmit = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.lbCommandName = new System.Windows.Forms.Label();
            this.tbCommandName = new System.Windows.Forms.TextBox();
            this.btReset = new System.Windows.Forms.Button();
            this.cbTarget = new System.Windows.Forms.CheckBox();
            this.cbConfirmation = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // cbToggle
            // 
            this.cbToggle.AutoSize = true;
            this.cbToggle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbToggle.Location = new System.Drawing.Point(3, 30);
            this.cbToggle.Name = "cbToggle";
            this.cbToggle.Size = new System.Drawing.Size(45, 16);
            this.cbToggle.TabIndex = 1;
            this.cbToggle.Text = "토글";
            this.cbToggle.UseVisualStyleBackColor = true;
            this.cbToggle.CheckedChanged += new System.EventHandler(this.cbToggle_CheckedChanged);
            // 
            // cbArgument
            // 
            this.cbArgument.AutoSize = true;
            this.cbArgument.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbArgument.Location = new System.Drawing.Point(52, 30);
            this.cbArgument.Name = "cbArgument";
            this.cbArgument.Size = new System.Drawing.Size(45, 16);
            this.cbArgument.TabIndex = 1;
            this.cbArgument.Text = "인수";
            this.cbArgument.UseVisualStyleBackColor = true;
            // 
            // lbCommand_1
            // 
            this.lbCommand_1.AutoSize = true;
            this.lbCommand_1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbCommand_1.Location = new System.Drawing.Point(1, 49);
            this.lbCommand_1.Name = "lbCommand_1";
            this.lbCommand_1.Size = new System.Drawing.Size(69, 12);
            this.lbCommand_1.TabIndex = 2;
            this.lbCommand_1.Text = "실행 명령어";
            // 
            // tbCommand_1
            // 
            this.tbCommand_1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbCommand_1.Location = new System.Drawing.Point(2, 64);
            this.tbCommand_1.MaxLength = 2147483647;
            this.tbCommand_1.Multiline = true;
            this.tbCommand_1.Name = "tbCommand_1";
            this.tbCommand_1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbCommand_1.Size = new System.Drawing.Size(259, 340);
            this.tbCommand_1.TabIndex = 3;
            this.tbCommand_1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbSelectAll);
            // 
            // lbCommand_2
            // 
            this.lbCommand_2.AutoSize = true;
            this.lbCommand_2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbCommand_2.Location = new System.Drawing.Point(282, 49);
            this.lbCommand_2.Name = "lbCommand_2";
            this.lbCommand_2.Size = new System.Drawing.Size(97, 12);
            this.lbCommand_2.TabIndex = 2;
            this.lbCommand_2.Text = "토글 해제 명령어";
            this.lbCommand_2.Visible = false;
            // 
            // tbCommand_2
            // 
            this.tbCommand_2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbCommand_2.Location = new System.Drawing.Point(283, 64);
            this.tbCommand_2.MaxLength = 2147483647;
            this.tbCommand_2.Multiline = true;
            this.tbCommand_2.Name = "tbCommand_2";
            this.tbCommand_2.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbCommand_2.Size = new System.Drawing.Size(259, 340);
            this.tbCommand_2.TabIndex = 4;
            this.tbCommand_2.Visible = false;
            this.tbCommand_2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbSelectAll);
            // 
            // btSubmit
            // 
            this.btSubmit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btSubmit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btSubmit.Location = new System.Drawing.Point(152, 2);
            this.btSubmit.Name = "btSubmit";
            this.btSubmit.Size = new System.Drawing.Size(53, 21);
            this.btSubmit.TabIndex = 5;
            this.btSubmit.Text = "적용";
            this.btSubmit.UseVisualStyleBackColor = true;
            this.btSubmit.Click += new System.EventHandler(this.btSubmit_Click);
            // 
            // btCancel
            // 
            this.btCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btCancel.Location = new System.Drawing.Point(206, 2);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(53, 21);
            this.btCancel.TabIndex = 6;
            this.btCancel.Text = "취소";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // lbCommandName
            // 
            this.lbCommandName.AutoSize = true;
            this.lbCommandName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbCommandName.Location = new System.Drawing.Point(1, 6);
            this.lbCommandName.Name = "lbCommandName";
            this.lbCommandName.Size = new System.Drawing.Size(61, 12);
            this.lbCommandName.TabIndex = 6;
            this.lbCommandName.Text = "명령 이름:";
            // 
            // tbCommandName
            // 
            this.tbCommandName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbCommandName.Location = new System.Drawing.Point(63, 2);
            this.tbCommandName.MaxLength = 20;
            this.tbCommandName.Multiline = true;
            this.tbCommandName.Name = "tbCommandName";
            this.tbCommandName.Size = new System.Drawing.Size(88, 21);
            this.tbCommandName.TabIndex = 0;
            this.tbCommandName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbSelectAll);
            // 
            // btReset
            // 
            this.btReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btReset.Location = new System.Drawing.Point(206, 28);
            this.btReset.Name = "btReset";
            this.btReset.Size = new System.Drawing.Size(53, 21);
            this.btReset.TabIndex = 7;
            this.btReset.Text = "초기화";
            this.btReset.UseVisualStyleBackColor = true;
            this.btReset.Click += new System.EventHandler(this.btReset_Click);
            // 
            // cbTarget
            // 
            this.cbTarget.AutoSize = true;
            this.cbTarget.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbTarget.Location = new System.Drawing.Point(101, 30);
            this.cbTarget.Name = "cbTarget";
            this.cbTarget.Size = new System.Drawing.Size(45, 16);
            this.cbTarget.TabIndex = 1;
            this.cbTarget.Text = "타겟";
            this.cbTarget.UseVisualStyleBackColor = true;
            // 
            // cbConfirmation
            // 
            this.cbConfirmation.AutoSize = true;
            this.cbConfirmation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbConfirmation.Location = new System.Drawing.Point(150, 30);
            this.cbConfirmation.Name = "cbConfirmation";
            this.cbConfirmation.Size = new System.Drawing.Size(45, 16);
            this.cbConfirmation.TabIndex = 1;
            this.cbConfirmation.Text = "확인";
            this.cbConfirmation.UseVisualStyleBackColor = true;
            // 
            // frmPreset
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(548, 413);
            this.Controls.Add(this.tbCommandName);
            this.Controls.Add(this.lbCommandName);
            this.Controls.Add(this.btReset);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btSubmit);
            this.Controls.Add(this.tbCommand_2);
            this.Controls.Add(this.tbCommand_1);
            this.Controls.Add(this.lbCommand_2);
            this.Controls.Add(this.lbCommand_1);
            this.Controls.Add(this.cbToggle);
            this.Controls.Add(this.cbArgument);
            this.Controls.Add(this.cbTarget);
            this.Controls.Add(this.cbConfirmation);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "frmPreset";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmPreset_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmPreset_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cbToggle;
        private System.Windows.Forms.CheckBox cbArgument;
        private System.Windows.Forms.Label lbCommand_1;
        private System.Windows.Forms.TextBox tbCommand_1;
        private System.Windows.Forms.Label lbCommand_2;
        private System.Windows.Forms.TextBox tbCommand_2;
        private System.Windows.Forms.Button btSubmit;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Label lbCommandName;
        private System.Windows.Forms.TextBox tbCommandName;
        private System.Windows.Forms.Button btReset;
        private System.Windows.Forms.CheckBox cbTarget;
        private System.Windows.Forms.CheckBox cbConfirmation;
    }
}