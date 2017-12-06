namespace JC.Lib.Demo
{
  partial class GPSCorrection
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
      this.label1 = new System.Windows.Forms.Label();
      this.txbX = new System.Windows.Forms.TextBox();
      this.txbY = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.button1 = new System.Windows.Forms.Button();
      this.label3 = new System.Windows.Forms.Label();
      this.textBox1 = new System.Windows.Forms.TextBox();
      this.label4 = new System.Windows.Forms.Label();
      this.txbX1 = new System.Windows.Forms.TextBox();
      this.label5 = new System.Windows.Forms.Label();
      this.txbY1 = new System.Windows.Forms.TextBox();
      this.button2 = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(215, 21);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(41, 12);
      this.label1.TabIndex = 0;
      this.label1.Text = "经度：";
      // 
      // txbX
      // 
      this.txbX.Location = new System.Drawing.Point(251, 16);
      this.txbX.Name = "txbX";
      this.txbX.Size = new System.Drawing.Size(140, 21);
      this.txbX.TabIndex = 1;
      // 
      // txbY
      // 
      this.txbY.Location = new System.Drawing.Point(60, 16);
      this.txbY.Name = "txbY";
      this.txbY.Size = new System.Drawing.Size(141, 21);
      this.txbY.TabIndex = 3;
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(24, 21);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(41, 12);
      this.label2.TabIndex = 2;
      this.label2.Text = "纬度：";
      // 
      // button1
      // 
      this.button1.Location = new System.Drawing.Point(416, 14);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(75, 23);
      this.button1.TabIndex = 4;
      this.button1.Text = "换算";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new System.EventHandler(this.button1_Click);
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(25, 95);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(41, 12);
      this.label3.TabIndex = 5;
      this.label3.Text = "结果：";
      // 
      // textBox1
      // 
      this.textBox1.Location = new System.Drawing.Point(61, 93);
      this.textBox1.Name = "textBox1";
      this.textBox1.Size = new System.Drawing.Size(140, 21);
      this.textBox1.TabIndex = 7;
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(215, 61);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(41, 12);
      this.label4.TabIndex = 0;
      this.label4.Text = "经度：";
      // 
      // txbX1
      // 
      this.txbX1.Location = new System.Drawing.Point(251, 56);
      this.txbX1.Name = "txbX1";
      this.txbX1.Size = new System.Drawing.Size(140, 21);
      this.txbX1.TabIndex = 1;
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(24, 61);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(41, 12);
      this.label5.TabIndex = 2;
      this.label5.Text = "纬度：";
      // 
      // txbY1
      // 
      this.txbY1.Location = new System.Drawing.Point(60, 56);
      this.txbY1.Name = "txbY1";
      this.txbY1.Size = new System.Drawing.Size(141, 21);
      this.txbY1.TabIndex = 3;
      // 
      // button2
      // 
      this.button2.Location = new System.Drawing.Point(416, 54);
      this.button2.Name = "button2";
      this.button2.Size = new System.Drawing.Size(75, 23);
      this.button2.TabIndex = 4;
      this.button2.Text = "距离";
      this.button2.UseVisualStyleBackColor = true;
      this.button2.Click += new System.EventHandler(this.button2_Click);
      // 
      // GPSCorrection
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(517, 187);
      this.Controls.Add(this.textBox1);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.button2);
      this.Controls.Add(this.button1);
      this.Controls.Add(this.txbY1);
      this.Controls.Add(this.txbY);
      this.Controls.Add(this.label5);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.txbX1);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.txbX);
      this.Controls.Add(this.label1);
      this.Name = "GPSCorrection";
      this.Text = "Form1";
      this.ResumeLayout(false);
      this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txbX;
        private System.Windows.Forms.TextBox txbY;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txbX1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txbY1;
        private System.Windows.Forms.Button button2;

    }
}

