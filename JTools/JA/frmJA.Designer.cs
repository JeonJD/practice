namespace JA
{
    partial class frmJA
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbServer = new System.Windows.Forms.ListBox();
            this.btExecute = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tbAccount = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lbServer
            // 
            this.lbServer.FormattingEnabled = true;
            this.lbServer.ItemHeight = 12;
            this.lbServer.Items.AddRange(new object[] {
            "QA1",
            "QA1_Branch",
            "QA2",
            "QA2_QA_GrandOpen",
            "QA2_QA_OBT"});
            this.lbServer.Location = new System.Drawing.Point(12, 10);
            this.lbServer.Name = "lbServer";
            this.lbServer.Size = new System.Drawing.Size(120, 40);
            this.lbServer.TabIndex = 0;
            // 
            // btExecute
            // 
            this.btExecute.Location = new System.Drawing.Point(140, 27);
            this.btExecute.Name = "btExecute";
            this.btExecute.Size = new System.Drawing.Size(158, 24);
            this.btExecute.TabIndex = 1;
            this.btExecute.Text = "파괴";
            this.btExecute.UseVisualStyleBackColor = true;
            this.btExecute.Click += new System.EventHandler(this.btExecute_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(138, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "account:";
            // 
            // tbAccount
            // 
            this.tbAccount.Location = new System.Drawing.Point(198, 6);
            this.tbAccount.Name = "tbAccount";
            this.tbAccount.Size = new System.Drawing.Size(100, 21);
            this.tbAccount.TabIndex = 3;
            // 
            // frmJA
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(309, 56);
            this.Controls.Add(this.tbAccount);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btExecute);
            this.Controls.Add(this.lbServer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmJA";
            this.Text = "업적파괴자";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbServer;
        private System.Windows.Forms.Button btExecute;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbAccount;
    }
}

