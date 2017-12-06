namespace JC.Lib.Demo
{
  partial class ImgBatch
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
      this.btnResFolderBrower = new System.Windows.Forms.Button();
      this.txtResDir = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.rtbMsg = new System.Windows.Forms.RichTextBox();
      this.button1 = new System.Windows.Forms.Button();
      this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
      this.fileSystemWatcher1 = new System.IO.FileSystemWatcher();
      ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).BeginInit();
      this.SuspendLayout();
      // 
      // btnResFolderBrower
      // 
      this.btnResFolderBrower.Location = new System.Drawing.Point(413, 221);
      this.btnResFolderBrower.Name = "btnResFolderBrower";
      this.btnResFolderBrower.Size = new System.Drawing.Size(33, 23);
      this.btnResFolderBrower.TabIndex = 17;
      this.btnResFolderBrower.Text = "...";
      this.btnResFolderBrower.UseVisualStyleBackColor = true;
      this.btnResFolderBrower.Click += new System.EventHandler(this.btnResFolderBrower_Click);
      // 
      // txtResDir
      // 
      this.txtResDir.Location = new System.Drawing.Point(70, 221);
      this.txtResDir.Name = "txtResDir";
      this.txtResDir.Size = new System.Drawing.Size(337, 21);
      this.txtResDir.TabIndex = 15;
      this.txtResDir.Text = "C:\\Users\\Administrator\\Desktop\\t";
      this.txtResDir.TextChanged += new System.EventHandler(this.txtResDir_TextChanged);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(3, 230);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(53, 12);
      this.label1.TabIndex = 13;
      this.label1.Text = "源文件夹";
      this.label1.Click += new System.EventHandler(this.label1_Click);
      // 
      // rtbMsg
      // 
      this.rtbMsg.Location = new System.Drawing.Point(0, 3);
      this.rtbMsg.Name = "rtbMsg";
      this.rtbMsg.Size = new System.Drawing.Size(516, 186);
      this.rtbMsg.TabIndex = 12;
      this.rtbMsg.Text = "";
      this.rtbMsg.TextChanged += new System.EventHandler(this.rtbMsg_TextChanged);
      // 
      // button1
      // 
      this.button1.BackColor = System.Drawing.Color.LightSeaGreen;
      this.button1.Location = new System.Drawing.Point(452, 197);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(59, 51);
      this.button1.TabIndex = 11;
      this.button1.Text = "开始处理图片";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new System.EventHandler(this.button1_Click);
      // 
      // fileSystemWatcher1
      // 
      this.fileSystemWatcher1.EnableRaisingEvents = true;
      this.fileSystemWatcher1.SynchronizingObject = this;
      // 
      // ImgBatch
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(517, 456);
      this.Controls.Add(this.btnResFolderBrower);
      this.Controls.Add(this.txtResDir);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.rtbMsg);
      this.Controls.Add(this.button1);
      this.Name = "ImgBatch";
      this.Text = "ImgBatch";
      ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button btnResFolderBrower;
    private System.Windows.Forms.TextBox txtResDir;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.RichTextBox rtbMsg;
    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
    private System.IO.FileSystemWatcher fileSystemWatcher1;

  }
}