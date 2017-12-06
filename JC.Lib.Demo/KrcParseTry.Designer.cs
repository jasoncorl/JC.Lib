namespace testDemo
{
  partial class KrcParseTry
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
      this.rtbMsg = new System.Windows.Forms.RichTextBox();
      this.button1 = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // rtbMsg
      // 
      this.rtbMsg.Location = new System.Drawing.Point(8, 45);
      this.rtbMsg.Name = "rtbMsg";
      this.rtbMsg.Size = new System.Drawing.Size(437, 270);
      this.rtbMsg.TabIndex = 4;
      this.rtbMsg.Text = "";
      // 
      // button1
      // 
      this.button1.BackColor = System.Drawing.Color.LightSeaGreen;
      this.button1.Location = new System.Drawing.Point(8, 12);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(437, 27);
      this.button1.TabIndex = 3;
      this.button1.Text = "开始解析歌词";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new System.EventHandler(this.button1_Click);
      // 
      // KrcParseTry
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(457, 327);
      this.Controls.Add(this.rtbMsg);
      this.Controls.Add(this.button1);
      this.Name = "KrcParseTry";
      this.Text = "KrcParseTry";
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.RichTextBox rtbMsg;
    private System.Windows.Forms.Button button1;
  }
}