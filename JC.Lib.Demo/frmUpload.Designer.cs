namespace JC.Lib.Demo
{
    partial class frmUpload
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
          this.btnUpload = new System.Windows.Forms.Button();
          this.rtbMsg = new System.Windows.Forms.RichTextBox();
          this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
          this.ClientFolder = new System.Windows.Forms.TextBox();
          this.label1 = new System.Windows.Forms.Label();
          this.UploadType = new System.Windows.Forms.ComboBox();
          this.label2 = new System.Windows.Forms.Label();
          this.button1 = new System.Windows.Forms.Button();
          this.SuspendLayout();
          // 
          // btnUpload
          // 
          this.btnUpload.Location = new System.Drawing.Point(446, 3);
          this.btnUpload.Name = "btnUpload";
          this.btnUpload.Size = new System.Drawing.Size(62, 51);
          this.btnUpload.TabIndex = 0;
          this.btnUpload.Text = "Upload";
          this.btnUpload.UseVisualStyleBackColor = true;
          this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
          // 
          // rtbMsg
          // 
          this.rtbMsg.Location = new System.Drawing.Point(2, 60);
          this.rtbMsg.Name = "rtbMsg";
          this.rtbMsg.Size = new System.Drawing.Size(516, 315);
          this.rtbMsg.TabIndex = 1;
          this.rtbMsg.Text = "";
          // 
          // ClientFolder
          // 
          this.ClientFolder.Location = new System.Drawing.Point(83, 3);
          this.ClientFolder.Name = "ClientFolder";
          this.ClientFolder.Size = new System.Drawing.Size(317, 21);
          this.ClientFolder.TabIndex = 2;
          // 
          // label1
          // 
          this.label1.AutoSize = true;
          this.label1.Location = new System.Drawing.Point(12, 9);
          this.label1.Name = "label1";
          this.label1.Size = new System.Drawing.Size(65, 12);
          this.label1.TabIndex = 3;
          this.label1.Text = "上传文件夹";
          // 
          // UploadType
          // 
          this.UploadType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
          this.UploadType.FormattingEnabled = true;
          this.UploadType.Location = new System.Drawing.Point(83, 30);
          this.UploadType.Name = "UploadType";
          this.UploadType.Size = new System.Drawing.Size(100, 20);
          this.UploadType.TabIndex = 4;
          // 
          // label2
          // 
          this.label2.AutoSize = true;
          this.label2.Location = new System.Drawing.Point(24, 33);
          this.label2.Name = "label2";
          this.label2.Size = new System.Drawing.Size(53, 12);
          this.label2.TabIndex = 5;
          this.label2.Text = "上传类型";
          // 
          // button1
          // 
          this.button1.Location = new System.Drawing.Point(407, 0);
          this.button1.Name = "button1";
          this.button1.Size = new System.Drawing.Size(33, 23);
          this.button1.TabIndex = 6;
          this.button1.Text = "...";
          this.button1.UseVisualStyleBackColor = true;
          this.button1.Click += new System.EventHandler(this.button1_Click);
          // 
          // frmUpload
          // 
          this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
          this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
          this.ClientSize = new System.Drawing.Size(520, 376);
          this.Controls.Add(this.button1);
          this.Controls.Add(this.label2);
          this.Controls.Add(this.UploadType);
          this.Controls.Add(this.label1);
          this.Controls.Add(this.ClientFolder);
          this.Controls.Add(this.rtbMsg);
          this.Controls.Add(this.btnUpload);
          this.Name = "frmUpload";
          this.Text = "文件夹批量上传";
          this.Load += new System.EventHandler(this.frmUploadDemo_Load);
          this.ResumeLayout(false);
          this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.RichTextBox rtbMsg;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.TextBox ClientFolder;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox UploadType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
    }
}

