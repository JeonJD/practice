namespace DoItDoIt
{
    partial class frmDoItDoIt
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDoItDoIt));
            this.tbOpi = new System.Windows.Forms.TrackBar();
            this.cbHotkey = new System.Windows.Forms.CheckBox();
            this.cbAlwaysOnTop = new System.Windows.Forms.CheckBox();
            this.btHidden = new System.Windows.Forms.Button();
            this.btExit = new System.Windows.Forms.Button();
            this.lstItemsList = new System.Windows.Forms.ListBox();
            this.btConn = new System.Windows.Forms.Button();
            this.tbPort = new System.Windows.Forms.TextBox();
            this.tbInputItems = new System.Windows.Forms.TextBox();
            this.btAddITems = new System.Windows.Forms.Button();
            this.btDeleteITems = new System.Windows.Forms.Button();
            this.tbExCommand = new System.Windows.Forms.TextBox();
            this.tbExSubCommand = new System.Windows.Forms.TextBox();
            this.btReset = new System.Windows.Forms.Button();
            this.btStart = new System.Windows.Forms.Button();
            this.btNext = new System.Windows.Forms.Button();
            this.tbSubTurn = new System.Windows.Forms.TextBox();
            this.cbSubTurn = new System.Windows.Forms.CheckBox();
            this.lbCommand = new System.Windows.Forms.Label();
            this.lbList = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.tbOpi)).BeginInit();
            this.SuspendLayout();
            // 
            // tbOpi
            // 
            this.tbOpi.LargeChange = 1;
            this.tbOpi.Location = new System.Drawing.Point(304, 0);
            this.tbOpi.Minimum = 6;
            this.tbOpi.Name = "tbOpi";
            this.tbOpi.Size = new System.Drawing.Size(68, 45);
            this.tbOpi.TabIndex = 0;
            this.tbOpi.TickStyle = System.Windows.Forms.TickStyle.None;
            this.tbOpi.Value = 8;
            this.tbOpi.Scroll += new System.EventHandler(this.tbOpi_Scroll);
            // 
            // cbHotkey
            // 
            this.cbHotkey.AutoSize = true;
            this.cbHotkey.Checked = true;
            this.cbHotkey.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbHotkey.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbHotkey.Font = new System.Drawing.Font("굴림", 8F);
            this.cbHotkey.Location = new System.Drawing.Point(132, 0);
            this.cbHotkey.Name = "cbHotkey";
            this.cbHotkey.Size = new System.Drawing.Size(61, 15);
            this.cbHotkey.TabIndex = 0;
            this.cbHotkey.Text = "HotKey";
            this.cbHotkey.UseVisualStyleBackColor = true;
            this.cbHotkey.CheckedChanged += new System.EventHandler(this.cbHotkey_CheckedChanged);
            // 
            // cbAlwaysOnTop
            // 
            this.cbAlwaysOnTop.AutoSize = true;
            this.cbAlwaysOnTop.Checked = true;
            this.cbAlwaysOnTop.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAlwaysOnTop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbAlwaysOnTop.Font = new System.Drawing.Font("굴림", 8F);
            this.cbAlwaysOnTop.Location = new System.Drawing.Point(199, 0);
            this.cbAlwaysOnTop.Name = "cbAlwaysOnTop";
            this.cbAlwaysOnTop.Size = new System.Drawing.Size(99, 15);
            this.cbAlwaysOnTop.TabIndex = 0;
            this.cbAlwaysOnTop.Text = "AlwaysOnTop";
            this.cbAlwaysOnTop.UseVisualStyleBackColor = true;
            this.cbAlwaysOnTop.CheckedChanged += new System.EventHandler(this.cbAlwaysOnTop_CheckedChanged);
            // 
            // btHidden
            // 
            this.btHidden.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btHidden.Font = new System.Drawing.Font("굴림", 9F);
            this.btHidden.Location = new System.Drawing.Point(374, 0);
            this.btHidden.Name = "btHidden";
            this.btHidden.Size = new System.Drawing.Size(55, 21);
            this.btHidden.TabIndex = 0;
            this.btHidden.Text = "숨기기";
            this.btHidden.UseVisualStyleBackColor = true;
            this.btHidden.Click += new System.EventHandler(this.btHidden_Click);
            // 
            // btExit
            // 
            this.btExit.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btExit.Location = new System.Drawing.Point(429, 0);
            this.btExit.Name = "btExit";
            this.btExit.Size = new System.Drawing.Size(55, 21);
            this.btExit.TabIndex = 0;
            this.btExit.Text = "종료";
            this.btExit.UseVisualStyleBackColor = true;
            this.btExit.Click += new System.EventHandler(this.btExit_Click);
            // 
            // lstItemsList
            // 
            this.lstItemsList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstItemsList.FormattingEnabled = true;
            this.lstItemsList.ItemHeight = 12;
            this.lstItemsList.Location = new System.Drawing.Point(14, 64);
            this.lstItemsList.Name = "lstItemsList";
            this.lstItemsList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstItemsList.Size = new System.Drawing.Size(200, 434);
            this.lstItemsList.TabIndex = 0;
            this.lstItemsList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstItemsList_KeyDown);
            // 
            // btConn
            // 
            this.btConn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btConn.Location = new System.Drawing.Point(0, 0);
            this.btConn.Name = "btConn";
            this.btConn.Size = new System.Drawing.Size(44, 21);
            this.btConn.TabIndex = 0;
            this.btConn.Text = "연결";
            this.btConn.UseVisualStyleBackColor = true;
            this.btConn.Click += new System.EventHandler(this.btConn_Click);
            // 
            // tbPort
            // 
            this.tbPort.Font = new System.Drawing.Font("굴림", 8F);
            this.tbPort.Location = new System.Drawing.Point(50, 1);
            this.tbPort.Name = "tbPort";
            this.tbPort.Size = new System.Drawing.Size(34, 20);
            this.tbPort.TabIndex = 0;
            this.tbPort.Text = "9111";
            this.tbPort.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbPort_KeyDown);
            // 
            // tbInputItems
            // 
            this.tbInputItems.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbInputItems.Location = new System.Drawing.Point(14, 554);
            this.tbInputItems.MaxLength = 999999;
            this.tbInputItems.Multiline = true;
            this.tbInputItems.Name = "tbInputItems";
            this.tbInputItems.Size = new System.Drawing.Size(200, 196);
            this.tbInputItems.TabIndex = 0;
            this.tbInputItems.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbInputItems_KeyDown);
            // 
            // btAddITems
            // 
            this.btAddITems.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btAddITems.Location = new System.Drawing.Point(14, 513);
            this.btAddITems.Name = "btAddITems";
            this.btAddITems.Size = new System.Drawing.Size(60, 23);
            this.btAddITems.TabIndex = 0;
            this.btAddITems.Text = "추가 ↑";
            this.btAddITems.UseVisualStyleBackColor = true;
            this.btAddITems.Click += new System.EventHandler(this.btAddITems_Click);
            // 
            // btDeleteITems
            // 
            this.btDeleteITems.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btDeleteITems.Location = new System.Drawing.Point(84, 513);
            this.btDeleteITems.Name = "btDeleteITems";
            this.btDeleteITems.Size = new System.Drawing.Size(60, 23);
            this.btDeleteITems.TabIndex = 0;
            this.btDeleteITems.Text = "삭제";
            this.btDeleteITems.UseVisualStyleBackColor = true;
            this.btDeleteITems.Click += new System.EventHandler(this.btDeleteITems_Click);
            // 
            // tbExCommand
            // 
            this.tbExCommand.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbExCommand.Location = new System.Drawing.Point(272, 64);
            this.tbExCommand.Multiline = true;
            this.tbExCommand.Name = "tbExCommand";
            this.tbExCommand.Size = new System.Drawing.Size(200, 300);
            this.tbExCommand.TabIndex = 0;
            this.tbExCommand.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbExCommand_KeyDown);
            // 
            // tbExSubCommand
            // 
            this.tbExSubCommand.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbExSubCommand.Location = new System.Drawing.Point(272, 404);
            this.tbExSubCommand.Multiline = true;
            this.tbExSubCommand.Name = "tbExSubCommand";
            this.tbExSubCommand.Size = new System.Drawing.Size(200, 300);
            this.tbExSubCommand.TabIndex = 0;
            this.tbExSubCommand.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbExSubCommand_KeyDown);
            // 
            // btReset
            // 
            this.btReset.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btReset.Location = new System.Drawing.Point(154, 513);
            this.btReset.Name = "btReset";
            this.btReset.Size = new System.Drawing.Size(60, 23);
            this.btReset.TabIndex = 0;
            this.btReset.Text = "초기화";
            this.btReset.UseVisualStyleBackColor = true;
            this.btReset.Click += new System.EventHandler(this.btReset_Click);
            // 
            // btStart
            // 
            this.btStart.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btStart.Location = new System.Drawing.Point(271, 710);
            this.btStart.Name = "btStart";
            this.btStart.Size = new System.Drawing.Size(85, 40);
            this.btStart.TabIndex = 0;
            this.btStart.Text = "끝까지 또 해\r\n[Alt + P]";
            this.btStart.UseVisualStyleBackColor = true;
            this.btStart.Click += new System.EventHandler(this.btStart_Click);
            // 
            // btNext
            // 
            this.btNext.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btNext.Location = new System.Drawing.Point(387, 710);
            this.btNext.Name = "btNext";
            this.btNext.Size = new System.Drawing.Size(85, 40);
            this.btNext.TabIndex = 0;
            this.btNext.Text = "하나씩 또 해\r\n[Alt+X]";
            this.btNext.UseVisualStyleBackColor = true;
            this.btNext.Click += new System.EventHandler(this.btNext_Click);
            // 
            // tbSubTurn
            // 
            this.tbSubTurn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbSubTurn.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.tbSubTurn.Location = new System.Drawing.Point(272, 377);
            this.tbSubTurn.Name = "tbSubTurn";
            this.tbSubTurn.Size = new System.Drawing.Size(40, 21);
            this.tbSubTurn.TabIndex = 0;
            this.tbSubTurn.TextChanged += new System.EventHandler(this.tbSubTurn_TextChanged);
            // 
            // cbSubTurn
            // 
            this.cbSubTurn.AutoSize = true;
            this.cbSubTurn.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbSubTurn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cbSubTurn.Location = new System.Drawing.Point(317, 382);
            this.cbSubTurn.Name = "cbSubTurn";
            this.cbSubTurn.Size = new System.Drawing.Size(158, 16);
            this.cbSubTurn.TabIndex = 0;
            this.cbSubTurn.Text = "회마다 아래 명령어 또 해";
            this.cbSubTurn.UseVisualStyleBackColor = true;
            // 
            // lbCommand
            // 
            this.lbCommand.AutoSize = true;
            this.lbCommand.Location = new System.Drawing.Point(272, 49);
            this.lbCommand.Name = "lbCommand";
            this.lbCommand.Size = new System.Drawing.Size(169, 12);
            this.lbCommand.TabIndex = 1;
            this.lbCommand.Text = "목록에 대해 아래 명령어 또 해";
            // 
            // lbList
            // 
            this.lbList.AutoSize = true;
            this.lbList.Location = new System.Drawing.Point(12, 49);
            this.lbList.Name = "lbList";
            this.lbList.Size = new System.Drawing.Size(61, 12);
            this.lbList.TabIndex = 1;
            this.lbList.Text = "또 할 목록";
            // 
            // frmDoItDoIt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 762);
            this.Controls.Add(this.lbList);
            this.Controls.Add(this.lbCommand);
            this.Controls.Add(this.cbSubTurn);
            this.Controls.Add(this.tbSubTurn);
            this.Controls.Add(this.btNext);
            this.Controls.Add(this.btStart);
            this.Controls.Add(this.btReset);
            this.Controls.Add(this.btDeleteITems);
            this.Controls.Add(this.btAddITems);
            this.Controls.Add(this.tbExSubCommand);
            this.Controls.Add(this.tbExCommand);
            this.Controls.Add(this.tbInputItems);
            this.Controls.Add(this.btConn);
            this.Controls.Add(this.tbPort);
            this.Controls.Add(this.lstItemsList);
            this.Controls.Add(this.cbHotkey);
            this.Controls.Add(this.cbAlwaysOnTop);
            this.Controls.Add(this.btHidden);
            this.Controls.Add(this.btExit);
            this.Controls.Add(this.tbOpi);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "frmDoItDoIt";
            this.Opacity = 0.8D;
            this.ShowIcon = false;
            this.Text = "DoItDoIt";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmDoItDoIt_FormClosing);
            this.Load += new System.EventHandler(this.frmDoItDoIt_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tbOpi)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar tbOpi;
        private System.Windows.Forms.CheckBox cbHotkey;
        private System.Windows.Forms.CheckBox cbAlwaysOnTop;
        private System.Windows.Forms.Button btHidden;
        private System.Windows.Forms.Button btExit;
        private System.Windows.Forms.ListBox lstItemsList;
        private System.Windows.Forms.Button btConn;
        private System.Windows.Forms.TextBox tbPort;
        private System.Windows.Forms.TextBox tbInputItems;
        private System.Windows.Forms.Button btAddITems;
        private System.Windows.Forms.Button btDeleteITems;
        private System.Windows.Forms.TextBox tbExCommand;
        private System.Windows.Forms.TextBox tbExSubCommand;
        private System.Windows.Forms.Button btReset;
        private System.Windows.Forms.Button btStart;
        private System.Windows.Forms.Button btNext;
        private System.Windows.Forms.TextBox tbSubTurn;
        private System.Windows.Forms.CheckBox cbSubTurn;
        private System.Windows.Forms.Label lbCommand;
        private System.Windows.Forms.Label lbList;
    }
}

