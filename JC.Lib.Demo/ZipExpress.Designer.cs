namespace JC.Lib.Demo
{
  partial class ZipExpress
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
      this.button2 = new System.Windows.Forms.Button();
      this.button3 = new System.Windows.Forms.Button();
      this.button4 = new System.Windows.Forms.Button();
      this.button5 = new System.Windows.Forms.Button();
      this.button6 = new System.Windows.Forms.Button();
      this.listView1 = new System.Windows.Forms.ListView();
      this.SuspendLayout();
      // 
      // button1
      // 
      this.button1.Location = new System.Drawing.Point(12, 12);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(40, 33);
      this.button1.TabIndex = 0;
      this.button1.Text = "新建";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new System.EventHandler(this.button1_Click);
      // 
      // button2
      // 
      this.button2.Location = new System.Drawing.Point(58, 12);
      this.button2.Name = "button2";
      this.button2.Size = new System.Drawing.Size(40, 33);
      this.button2.TabIndex = 1;
      this.button2.Text = "打开";
      this.button2.UseVisualStyleBackColor = true;
      this.button2.Click += new System.EventHandler(this.button2_Click);
      // 
      // button3
      // 
      this.button3.Location = new System.Drawing.Point(104, 12);
      this.button3.Name = "button3";
      this.button3.Size = new System.Drawing.Size(40, 33);
      this.button3.TabIndex = 4;
      this.button3.Text = "添加文件";
      this.button3.UseVisualStyleBackColor = true;
      this.button3.Click += new System.EventHandler(this.button3_Click);
      // 
      // button4
      // 
      this.button4.Location = new System.Drawing.Point(150, 12);
      this.button4.Name = "button4";
      this.button4.Size = new System.Drawing.Size(40, 33);
      this.button4.TabIndex = 5;
      this.button4.Text = "保存";
      this.button4.UseVisualStyleBackColor = true;
      this.button4.Click += new System.EventHandler(this.button4_Click);
      // 
      // button5
      // 
      this.button5.Location = new System.Drawing.Point(196, 12);
      this.button5.Name = "button5";
      this.button5.Size = new System.Drawing.Size(40, 33);
      this.button5.TabIndex = 6;
      this.button5.Text = "解压";
      this.button5.UseVisualStyleBackColor = true;
      this.button5.Click += new System.EventHandler(this.button5_Click);
      // 
      // button6
      // 
      this.button6.Location = new System.Drawing.Point(242, 12);
      this.button6.Name = "button6";
      this.button6.Size = new System.Drawing.Size(40, 33);
      this.button6.TabIndex = 7;
      this.button6.Text = "退出";
      this.button6.UseVisualStyleBackColor = true;
      this.button6.Click += new System.EventHandler(this.button6_Click);
      // 
      // listView1
      // 
      this.listView1.Location = new System.Drawing.Point(12, 51);
      this.listView1.Name = "listView1";
      this.listView1.Size = new System.Drawing.Size(459, 316);
      this.listView1.TabIndex = 3;
      this.listView1.UseCompatibleStateImageBehavior = false;
      // 
      // ZipExpress
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(483, 379);
      this.Controls.Add(this.button6);
      this.Controls.Add(this.button5);
      this.Controls.Add(this.button4);
      this.Controls.Add(this.button3);
      this.Controls.Add(this.listView1);
      this.Controls.Add(this.button2);
      this.Controls.Add(this.button1);
      this.MaximizeBox = false;
      this.Name = "ZipExpress";
      this.Text = "盈盛创富压缩管理工具";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ZipExpress_FormClosing);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.Button button2;
    private System.Windows.Forms.Button button3;
    private System.Windows.Forms.Button button4;
    private System.Windows.Forms.Button button5;
    private System.Windows.Forms.Button button6;
    private System.Windows.Forms.ListView listView1;
  }
}