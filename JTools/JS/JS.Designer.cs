namespace JS
{
    partial class Form_JSearch
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
        public void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_JSearch));
            this.cb_Tablelist = new System.Windows.Forms.ComboBox();
            this.lb_Jsonlist = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tb_JsonPath = new System.Windows.Forms.TextBox();
            this.lb_Jsonpath = new System.Windows.Forms.Label();
            this.bt_setPath = new System.Windows.Forms.Button();
            this.tb_search = new System.Windows.Forms.TextBox();
            this.bt_search = new System.Windows.Forms.Button();
            this.lb_result = new System.Windows.Forms.ListBox();
            this.bt_searchAll = new System.Windows.Forms.Button();
            this.pb_Searching = new System.Windows.Forms.ProgressBar();
            this.cb_string = new System.Windows.Forms.CheckBox();
            this.cbSession = new System.Windows.Forms.ComboBox();
            this.lbNotify = new System.Windows.Forms.Label();
            this.rb_Korean = new System.Windows.Forms.RadioButton();
            this.rb_English = new System.Windows.Forms.RadioButton();
            this.bt_stringCancel = new System.Windows.Forms.Button();
            this.gb_stringChange = new System.Windows.Forms.GroupBox();
            this.dgv_result = new JS.QuickDataGridView();
            this.panel1.SuspendLayout();
            this.gb_stringChange.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_result)).BeginInit();
            this.SuspendLayout();
            // 
            // cb_Tablelist
            // 
            this.cb_Tablelist.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cb_Tablelist.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cb_Tablelist.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.cb_Tablelist.Location = new System.Drawing.Point(525, 12);
            this.cb_Tablelist.Name = "cb_Tablelist";
            this.cb_Tablelist.Size = new System.Drawing.Size(308, 23);
            this.cb_Tablelist.TabIndex = 0;
            this.cb_Tablelist.Text = "ALL";
            // 
            // lb_Jsonlist
            // 
            this.lb_Jsonlist.AutoEllipsis = true;
            this.lb_Jsonlist.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lb_Jsonlist.Location = new System.Drawing.Point(480, 11);
            this.lb_Jsonlist.Name = "lb_Jsonlist";
            this.lb_Jsonlist.Size = new System.Drawing.Size(39, 24);
            this.lb_Jsonlist.TabIndex = 0;
            this.lb_Jsonlist.Text = "Table";
            this.lb_Jsonlist.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.tb_JsonPath);
            this.panel1.Controls.Add(this.lb_Jsonpath);
            this.panel1.Controls.Add(this.bt_setPath);
            this.panel1.Location = new System.Drawing.Point(12, 11);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(185, 24);
            this.panel1.TabIndex = 4;
            // 
            // tb_JsonPath
            // 
            this.tb_JsonPath.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tb_JsonPath.Location = new System.Drawing.Point(34, 3);
            this.tb_JsonPath.Name = "tb_JsonPath";
            this.tb_JsonPath.Size = new System.Drawing.Size(123, 16);
            this.tb_JsonPath.TabIndex = 1;
            // 
            // lb_Jsonpath
            // 
            this.lb_Jsonpath.AutoSize = true;
            this.lb_Jsonpath.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lb_Jsonpath.Location = new System.Drawing.Point(1, 3);
            this.lb_Jsonpath.Name = "lb_Jsonpath";
            this.lb_Jsonpath.Size = new System.Drawing.Size(36, 15);
            this.lb_Jsonpath.TabIndex = 0;
            this.lb_Jsonpath.Text = "Path:";
            // 
            // bt_setPath
            // 
            this.bt_setPath.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.bt_setPath.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.bt_setPath.Location = new System.Drawing.Point(158, 3);
            this.bt_setPath.Name = "bt_setPath";
            this.bt_setPath.Size = new System.Drawing.Size(24, 16);
            this.bt_setPath.TabIndex = 2;
            this.bt_setPath.Text = "...";
            this.bt_setPath.UseVisualStyleBackColor = true;
            this.bt_setPath.Click += new System.EventHandler(this.bt_setPath_Click);
            // 
            // tb_search
            // 
            this.tb_search.Location = new System.Drawing.Point(839, 12);
            this.tb_search.Name = "tb_search";
            this.tb_search.Size = new System.Drawing.Size(246, 23);
            this.tb_search.TabIndex = 1;
            this.tb_search.Enter += new System.EventHandler(this.tb_search_Enter);
            this.tb_search.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tb_search_KeyDown);
            // 
            // bt_search
            // 
            this.bt_search.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.bt_search.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.bt_search.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.bt_search.Location = new System.Drawing.Point(1088, 11);
            this.bt_search.Margin = new System.Windows.Forms.Padding(0);
            this.bt_search.Name = "bt_search";
            this.bt_search.Size = new System.Drawing.Size(52, 24);
            this.bt_search.TabIndex = 2;
            this.bt_search.Text = "Search";
            this.bt_search.UseVisualStyleBackColor = true;
            this.bt_search.Click += new System.EventHandler(this.bt_search_Click);
            // 
            // lb_result
            // 
            this.lb_result.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lb_result.FormattingEnabled = true;
            this.lb_result.HorizontalScrollbar = true;
            this.lb_result.ItemHeight = 15;
            this.lb_result.Location = new System.Drawing.Point(12, 51);
            this.lb_result.Name = "lb_result";
            this.lb_result.Size = new System.Drawing.Size(248, 709);
            this.lb_result.TabIndex = 3;
            this.lb_result.SelectedIndexChanged += new System.EventHandler(this.lb_result_SelectedIndexChanged);
            this.lb_result.DoubleClick += new System.EventHandler(this.lb_result_DoubleClick);
            // 
            // bt_searchAll
            // 
            this.bt_searchAll.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.bt_searchAll.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.bt_searchAll.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.bt_searchAll.Location = new System.Drawing.Point(1143, 11);
            this.bt_searchAll.Name = "bt_searchAll";
            this.bt_searchAll.Size = new System.Drawing.Size(207, 24);
            this.bt_searchAll.TabIndex = 9;
            this.bt_searchAll.UseVisualStyleBackColor = true;
            this.bt_searchAll.Visible = false;
            this.bt_searchAll.Click += new System.EventHandler(this.bt_searchAll_Click);
            // 
            // pb_Searching
            // 
            this.pb_Searching.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pb_Searching.Cursor = System.Windows.Forms.Cursors.Default;
            this.pb_Searching.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pb_Searching.Location = new System.Drawing.Point(12, 40);
            this.pb_Searching.Maximum = 1000;
            this.pb_Searching.Name = "pb_Searching";
            this.pb_Searching.Size = new System.Drawing.Size(1560, 10);
            this.pb_Searching.Step = 1;
            this.pb_Searching.TabIndex = 14;
            // 
            // cb_string
            // 
            this.cb_string.Location = new System.Drawing.Point(388, 14);
            this.cb_string.Name = "cb_string";
            this.cb_string.Size = new System.Drawing.Size(86, 19);
            this.cb_string.TabIndex = 12;
            this.cb_string.Text = "String 포함";
            this.cb_string.UseVisualStyleBackColor = true;
            this.cb_string.CheckedChanged += new System.EventHandler(this.cb_string_CheckedChanged);
            // 
            // cbSession
            // 
            this.cbSession.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSession.FormattingEnabled = true;
            this.cbSession.Location = new System.Drawing.Point(203, 12);
            this.cbSession.Name = "cbSession";
            this.cbSession.Size = new System.Drawing.Size(179, 23);
            this.cbSession.TabIndex = 15;
            this.cbSession.SelectionChangeCommitted += new System.EventHandler(this.cbSession_TextUpdate);
            // 
            // lbNotify
            // 
            this.lbNotify.AutoSize = true;
            this.lbNotify.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.lbNotify.Location = new System.Drawing.Point(1353, 5);
            this.lbNotify.Name = "lbNotify";
            this.lbNotify.Size = new System.Drawing.Size(219, 30);
            this.lbNotify.TabIndex = 16;
            this.lbNotify.Text = "검색 결과 테이블의 필드값을 더블클릭 \r\n하면 새 검색 키워드로 등록됩니다.";
            // 
            // rb_Korean
            // 
            this.rb_Korean.Location = new System.Drawing.Point(14, 19);
            this.rb_Korean.Name = "rb_Korean";
            this.rb_Korean.Size = new System.Drawing.Size(49, 19);
            this.rb_Korean.TabIndex = 0;
            this.rb_Korean.TabStop = true;
            this.rb_Korean.Text = "한글";
            this.rb_Korean.UseVisualStyleBackColor = true;
            this.rb_Korean.CheckedChanged += new System.EventHandler(this.rb_Korean_CheckedChanged);
            // 
            // rb_English
            // 
            this.rb_English.Location = new System.Drawing.Point(69, 19);
            this.rb_English.Name = "rb_English";
            this.rb_English.Size = new System.Drawing.Size(63, 19);
            this.rb_English.TabIndex = 1;
            this.rb_English.TabStop = true;
            this.rb_English.Text = "English";
            this.rb_English.UseVisualStyleBackColor = true;
            this.rb_English.CheckedChanged += new System.EventHandler(this.rb_English_CheckedChanged);
            // 
            // bt_stringCancel
            // 
            this.bt_stringCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.bt_stringCancel.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.bt_stringCancel.Location = new System.Drawing.Point(69, 0);
            this.bt_stringCancel.Margin = new System.Windows.Forms.Padding(0);
            this.bt_stringCancel.Name = "bt_stringCancel";
            this.bt_stringCancel.Size = new System.Drawing.Size(63, 20);
            this.bt_stringCancel.TabIndex = 2;
            this.bt_stringCancel.Text = "선택 해제";
            this.bt_stringCancel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.bt_stringCancel.UseVisualStyleBackColor = true;
            this.bt_stringCancel.Click += new System.EventHandler(this.bt_stringCancel_Click);
            // 
            // gb_stringChange
            // 
            this.gb_stringChange.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gb_stringChange.Controls.Add(this.bt_stringCancel);
            this.gb_stringChange.Controls.Add(this.rb_English);
            this.gb_stringChange.Controls.Add(this.rb_Korean);
            this.gb_stringChange.Enabled = false;
            this.gb_stringChange.Location = new System.Drawing.Point(1427, 40);
            this.gb_stringChange.Name = "gb_stringChange";
            this.gb_stringChange.Size = new System.Drawing.Size(145, 40);
            this.gb_stringChange.TabIndex = 13;
            this.gb_stringChange.TabStop = false;
            this.gb_stringChange.Text = "String치환";
            this.gb_stringChange.Visible = false;
            // 
            // dgv_result
            // 
            this.dgv_result.AllowUserToAddRows = false;
            this.dgv_result.AllowUserToResizeRows = false;
            this.dgv_result.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgv_result.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgv_result.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgv_result.Location = new System.Drawing.Point(266, 51);
            this.dgv_result.Name = "dgv_result";
            this.dgv_result.ReadOnly = true;
            this.dgv_result.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgv_result.RowTemplate.Height = 23;
            this.dgv_result.Size = new System.Drawing.Size(1306, 709);
            this.dgv_result.TabIndex = 10;
            this.dgv_result.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_result_CellEndEdit);
            this.dgv_result.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgv_result_CellMouseDoubleClick);
            // 
            // Form_JSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.ClientSize = new System.Drawing.Size(1584, 762);
            this.Controls.Add(this.lbNotify);
            this.Controls.Add(this.cbSession);
            this.Controls.Add(this.pb_Searching);
            this.Controls.Add(this.cb_string);
            this.Controls.Add(this.bt_searchAll);
            this.Controls.Add(this.lb_result);
            this.Controls.Add(this.bt_search);
            this.Controls.Add(this.tb_search);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.cb_Tablelist);
            this.Controls.Add(this.lb_Jsonlist);
            this.Controls.Add(this.dgv_result);
            this.Controls.Add(this.gb_stringChange);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MinimumSize = new System.Drawing.Size(1600, 800);
            this.Name = "Form_JSearch";
            this.Text = "JS";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form_JSearch_FormClosed);
            this.Load += new System.EventHandler(this.Form_JSearch_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form_JSearch_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.gb_stringChange.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_result)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cb_Tablelist;
        private System.Windows.Forms.Label lb_Jsonlist;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox tb_JsonPath;
        private System.Windows.Forms.Label lb_Jsonpath;
        private System.Windows.Forms.Button bt_setPath;
        private System.Windows.Forms.TextBox tb_search;
        private System.Windows.Forms.Button bt_search;
        private System.Windows.Forms.ListBox lb_result;
        //private System.Windows.Forms.DataGridView dgv_result;
        private System.Windows.Forms.Button bt_searchAll;
        private QuickDataGridView dgv_result;
        public System.Windows.Forms.ProgressBar pb_Searching;
        private System.Windows.Forms.CheckBox cb_string;
        private System.Windows.Forms.ComboBox cbSession;
        private System.Windows.Forms.Label lbNotify;
        private System.Windows.Forms.RadioButton rb_Korean;
        private System.Windows.Forms.RadioButton rb_English;
        private System.Windows.Forms.Button bt_stringCancel;
        private System.Windows.Forms.GroupBox gb_stringChange;
    }
}

