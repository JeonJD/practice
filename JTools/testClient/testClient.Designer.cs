namespace testClient
{
    partial class testClient
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
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.tbDebug = new System.Windows.Forms.TextBox();
            this.btDiscon = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tbDebug
            // 
            this.tbDebug.Location = new System.Drawing.Point(12, 42);
            this.tbDebug.Multiline = true;
            this.tbDebug.Name = "tbDebug";
            this.tbDebug.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbDebug.Size = new System.Drawing.Size(624, 319);
            this.tbDebug.TabIndex = 1;
            // 
            // btDiscon
            // 
            this.btDiscon.Location = new System.Drawing.Point(12, 13);
            this.btDiscon.Name = "btDiscon";
            this.btDiscon.Size = new System.Drawing.Size(75, 23);
            this.btDiscon.TabIndex = 0;
            this.btDiscon.Text = "연결 끊기";
            this.btDiscon.UseVisualStyleBackColor = true;
            this.btDiscon.Click += new System.EventHandler(this.btDiscon_Click);
            // 
            // testClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(652, 373);
            this.Controls.Add(this.tbDebug);
            this.Controls.Add(this.btDiscon);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "testClient";
            this.Text = "testClient";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.testClient_FormClosed);
            this.Load += new System.EventHandler(this.testClient_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbDebug;
        private System.Windows.Forms.Button btDiscon;
    }
}

