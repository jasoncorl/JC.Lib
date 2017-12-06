namespace testDemo
{
    partial class frmUploadDemo
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
          this.textBox1 = new System.Windows.Forms.TextBox();
          this.SuspendLayout();
          // 
          // btnUpload
          // 
          this.btnUpload.Location = new System.Drawing.Point(122, 12);
          this.btnUpload.Name = "btnUpload";
          this.btnUpload.Size = new System.Drawing.Size(122, 23);
          this.btnUpload.TabIndex = 0;
          this.btnUpload.Text = "Upload test";
          this.btnUpload.UseVisualStyleBackColor = true;
          this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
          // 
          // textBox1
          // 
          this.textBox1.Location = new System.Drawing.Point(12, 41);
          this.textBox1.Multiline = true;
          this.textBox1.Name = "textBox1";
          this.textBox1.Size = new System.Drawing.Size(496, 323);
          this.textBox1.TabIndex = 1;
          // 
          // frmUploadDemo
          // 
          this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
          this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
          this.ClientSize = new System.Drawing.Size(520, 376);
          this.Controls.Add(this.textBox1);
          this.Controls.Add(this.btnUpload);
          this.Name = "frmUploadDemo";
          this.Text = "Form1";
          this.Load += new System.EventHandler(this.frmUploadDemo_Load);
          this.ResumeLayout(false);
          this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.TextBox textBox1;
    }
}

