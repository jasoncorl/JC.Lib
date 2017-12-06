namespace JC.Lib.Demo
{
  partial class Thumbnails
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
      this.button1 = new System.Windows.Forms.Button();
      this.rtbMsg = new System.Windows.Forms.RichTextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.txtResDir = new System.Windows.Forms.TextBox();
      this.txtDescDir = new System.Windows.Forms.TextBox();
      this.txtMaxSize = new System.Windows.Forms.TextBox();
      this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
      this.btnResFolderBrower = new System.Windows.Forms.Button();
      this.btnDescFolderBrower = new System.Windows.Forms.Button();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this.fileSystemWatcher1 = new System.IO.FileSystemWatcher();
      this.tabControl1 = new System.Windows.Forms.TabControl();
      this.tabPage1 = new System.Windows.Forms.TabPage();
      this.groupBox3 = new System.Windows.Forms.GroupBox();
      this.tabPage2 = new System.Windows.Forms.TabPage();
      this.tabPage3 = new System.Windows.Forms.TabPage();
      ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).BeginInit();
      this.tabControl1.SuspendLayout();
      this.tabPage1.SuspendLayout();
      this.groupBox3.SuspendLayout();
      this.tabPage2.SuspendLayout();
      this.tabPage3.SuspendLayout();
      this.SuspendLayout();
      // 
      // button1
      // 
      this.button1.BackColor = System.Drawing.Color.LightSeaGreen;
      this.button1.Location = new System.Drawing.Point(452, 195);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(59, 51);
      this.button1.TabIndex = 0;
      this.button1.Text = "开始处理图片";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new System.EventHandler(this.button1_Click);
      // 
      // rtbMsg
      // 
      this.rtbMsg.Location = new System.Drawing.Point(0, 1);
      this.rtbMsg.Name = "rtbMsg";
      this.rtbMsg.Size = new System.Drawing.Size(516, 186);
      this.rtbMsg.TabIndex = 2;
      this.rtbMsg.Text = "";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(3, 202);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(53, 12);
      this.label1.TabIndex = 3;
      this.label1.Text = "源文件夹";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(3, 229);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(65, 12);
      this.label2.TabIndex = 4;
      this.label2.Text = "保存文件夹";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(6, 17);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(65, 12);
      this.label3.TabIndex = 5;
      this.label3.Text = "最大边尺寸";
      // 
      // txtResDir
      // 
      this.txtResDir.Location = new System.Drawing.Point(70, 193);
      this.txtResDir.Name = "txtResDir";
      this.txtResDir.Size = new System.Drawing.Size(337, 21);
      this.txtResDir.TabIndex = 6;
      this.txtResDir.Text = "D:\\我的资料库\\Desktop\\images";
      // 
      // txtDescDir
      // 
      this.txtDescDir.Location = new System.Drawing.Point(70, 220);
      this.txtDescDir.Name = "txtDescDir";
      this.txtDescDir.Size = new System.Drawing.Size(337, 21);
      this.txtDescDir.TabIndex = 7;
      this.txtDescDir.Text = "D:\\我的资料库\\Desktop\\images_ed";
      // 
      // txtMaxSize
      // 
      this.txtMaxSize.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.txtMaxSize.Location = new System.Drawing.Point(77, 14);
      this.txtMaxSize.Name = "txtMaxSize";
      this.txtMaxSize.Size = new System.Drawing.Size(34, 21);
      this.txtMaxSize.TabIndex = 8;
      this.txtMaxSize.Text = "0";
      // 
      // btnResFolderBrower
      // 
      this.btnResFolderBrower.Location = new System.Drawing.Point(413, 193);
      this.btnResFolderBrower.Name = "btnResFolderBrower";
      this.btnResFolderBrower.Size = new System.Drawing.Size(33, 23);
      this.btnResFolderBrower.TabIndex = 9;
      this.btnResFolderBrower.Text = "...";
      this.btnResFolderBrower.UseVisualStyleBackColor = true;
      this.btnResFolderBrower.Click += new System.EventHandler(this.btnResFolderBrower_Click);
      // 
      // btnDescFolderBrower
      // 
      this.btnDescFolderBrower.Location = new System.Drawing.Point(413, 218);
      this.btnDescFolderBrower.Name = "btnDescFolderBrower";
      this.btnDescFolderBrower.Size = new System.Drawing.Size(33, 23);
      this.btnDescFolderBrower.TabIndex = 10;
      this.btnDescFolderBrower.Text = "...";
      this.btnDescFolderBrower.UseVisualStyleBackColor = true;
      this.btnDescFolderBrower.Click += new System.EventHandler(this.btnDescFolderBrower_Click);
      // 
      // groupBox1
      // 
      this.groupBox1.Location = new System.Drawing.Point(3, 0);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(496, 173);
      this.groupBox1.TabIndex = 11;
      this.groupBox1.TabStop = false;
      // 
      // groupBox2
      // 
      this.groupBox2.Location = new System.Drawing.Point(3, 0);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size(493, 170);
      this.groupBox2.TabIndex = 14;
      this.groupBox2.TabStop = false;
      // 
      // fileSystemWatcher1
      // 
      this.fileSystemWatcher1.EnableRaisingEvents = true;
      this.fileSystemWatcher1.SynchronizingObject = this;
      // 
      // tabControl1
      // 
      this.tabControl1.Controls.Add(this.tabPage1);
      this.tabControl1.Controls.Add(this.tabPage2);
      this.tabControl1.Controls.Add(this.tabPage3);
      this.tabControl1.Location = new System.Drawing.Point(5, 252);
      this.tabControl1.Name = "tabControl1";
      this.tabControl1.SelectedIndex = 0;
      this.tabControl1.Size = new System.Drawing.Size(510, 202);
      this.tabControl1.TabIndex = 15;
      // 
      // tabPage1
      // 
      this.tabPage1.Controls.Add(this.groupBox3);
      this.tabPage1.Location = new System.Drawing.Point(4, 22);
      this.tabPage1.Name = "tabPage1";
      this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
      this.tabPage1.Size = new System.Drawing.Size(502, 176);
      this.tabPage1.TabIndex = 0;
      this.tabPage1.Text = "常规";
      this.tabPage1.UseVisualStyleBackColor = true;
      // 
      // groupBox3
      // 
      this.groupBox3.Controls.Add(this.label3);
      this.groupBox3.Controls.Add(this.txtMaxSize);
      this.groupBox3.Location = new System.Drawing.Point(3, 0);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.Size = new System.Drawing.Size(496, 175);
      this.groupBox3.TabIndex = 12;
      this.groupBox3.TabStop = false;
      // 
      // tabPage2
      // 
      this.tabPage2.Controls.Add(this.groupBox1);
      this.tabPage2.Location = new System.Drawing.Point(4, 22);
      this.tabPage2.Name = "tabPage2";
      this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
      this.tabPage2.Size = new System.Drawing.Size(502, 176);
      this.tabPage2.TabIndex = 1;
      this.tabPage2.Text = "文字水印";
      this.tabPage2.UseVisualStyleBackColor = true;
      // 
      // tabPage3
      // 
      this.tabPage3.Controls.Add(this.groupBox2);
      this.tabPage3.Location = new System.Drawing.Point(4, 22);
      this.tabPage3.Name = "tabPage3";
      this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
      this.tabPage3.Size = new System.Drawing.Size(502, 176);
      this.tabPage3.TabIndex = 2;
      this.tabPage3.Text = "图片水印";
      this.tabPage3.UseVisualStyleBackColor = true;
      // 
      // Thumbnails
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(517, 456);
      this.Controls.Add(this.tabControl1);
      this.Controls.Add(this.btnDescFolderBrower);
      this.Controls.Add(this.btnResFolderBrower);
      this.Controls.Add(this.txtDescDir);
      this.Controls.Add(this.txtResDir);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.rtbMsg);
      this.Controls.Add(this.button1);
      this.Name = "Thumbnails";
      this.Text = "批量处理图片";
      ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).EndInit();
      this.tabControl1.ResumeLayout(false);
      this.tabPage1.ResumeLayout(false);
      this.groupBox3.ResumeLayout(false);
      this.groupBox3.PerformLayout();
      this.tabPage2.ResumeLayout(false);
      this.tabPage3.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.RichTextBox rtbMsg;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox txtResDir;
    private System.Windows.Forms.TextBox txtDescDir;
    private System.Windows.Forms.TextBox txtMaxSize;
    private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
    private System.Windows.Forms.Button btnResFolderBrower;
    private System.Windows.Forms.Button btnDescFolderBrower;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.IO.FileSystemWatcher fileSystemWatcher1;
    private System.Windows.Forms.TabControl tabControl1;
    private System.Windows.Forms.TabPage tabPage1;
    private System.Windows.Forms.TabPage tabPage2;
    private System.Windows.Forms.TabPage tabPage3;
    private System.Windows.Forms.GroupBox groupBox3;
  }
}