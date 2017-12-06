namespace JC.Lib.Demo
{
  partial class MySqlDemo
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
      this.MySqlDataGridVw = new System.Windows.Forms.DataGridView();
      ((System.ComponentModel.ISupportInitialize)(this.MySqlDataGridVw)).BeginInit();
      this.SuspendLayout();
      // 
      // MySqlDataGridVw
      // 
      this.MySqlDataGridVw.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.MySqlDataGridVw.Location = new System.Drawing.Point(58, 23);
      this.MySqlDataGridVw.Name = "MySqlDataGridVw";
      this.MySqlDataGridVw.RowTemplate.Height = 23;
      this.MySqlDataGridVw.Size = new System.Drawing.Size(326, 248);
      this.MySqlDataGridVw.TabIndex = 0;
      // 
      // MySqlDemo
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(600, 348);
      this.Controls.Add(this.MySqlDataGridVw);
      this.Name = "MySqlDemo";
      this.Text = "MySqlDemo";
      this.Load += new System.EventHandler(this.MySqlDemo_Load);
      ((System.ComponentModel.ISupportInitialize)(this.MySqlDataGridVw)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.DataGridView MySqlDataGridVw;

  }
}