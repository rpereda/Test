namespace MonitorApp
{
    partial class frmMonitor
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
            this.btnStart = new System.Windows.Forms.Button();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.rb1 = new System.Windows.Forms.RadioButton();
            this.rb2 = new System.Windows.Forms.RadioButton();
            this.txtSite1 = new System.Windows.Forms.TextBox();
            this.txtSite2 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(16, 4);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(194, 23);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // txtLog
            // 
            this.txtLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLog.Location = new System.Drawing.Point(13, 63);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.Size = new System.Drawing.Size(422, 311);
            this.txtLog.TabIndex = 1;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(16, 34);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(194, 23);
            this.btnClear.TabIndex = 2;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // rb1
            // 
            this.rb1.AutoSize = true;
            this.rb1.Checked = true;
            this.rb1.Location = new System.Drawing.Point(237, 10);
            this.rb1.Name = "rb1";
            this.rb1.Size = new System.Drawing.Size(14, 13);
            this.rb1.TabIndex = 3;
            this.rb1.TabStop = true;
            this.rb1.UseVisualStyleBackColor = true;
            // 
            // rb2
            // 
            this.rb2.AutoSize = true;
            this.rb2.Location = new System.Drawing.Point(237, 39);
            this.rb2.Name = "rb2";
            this.rb2.Size = new System.Drawing.Size(14, 13);
            this.rb2.TabIndex = 4;
            this.rb2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rb2.UseVisualStyleBackColor = true;
            // 
            // txtSite1
            // 
            this.txtSite1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSite1.Font = new System.Drawing.Font("Berlin Sans FB", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSite1.Location = new System.Drawing.Point(257, 6);
            this.txtSite1.Name = "txtSite1";
            this.txtSite1.Size = new System.Drawing.Size(178, 22);
            this.txtSite1.TabIndex = 5;
            this.txtSite1.Text = "http://ebgdev03:81";
            this.txtSite1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtSite2
            // 
            this.txtSite2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSite2.Font = new System.Drawing.Font("Berlin Sans FB", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSite2.Location = new System.Drawing.Point(257, 36);
            this.txtSite2.Name = "txtSite2";
            this.txtSite2.Size = new System.Drawing.Size(178, 22);
            this.txtSite2.TabIndex = 6;
            this.txtSite2.Text = "http://main.mycheckngo.com";
            this.txtSite2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // frmMonitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(447, 386);
            this.Controls.Add(this.txtSite2);
            this.Controls.Add(this.txtSite1);
            this.Controls.Add(this.rb2);
            this.Controls.Add(this.rb1);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.btnStart);
            this.Name = "frmMonitor";
            this.Text = "Monitor";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.RadioButton rb1;
        private System.Windows.Forms.RadioButton rb2;
        private System.Windows.Forms.TextBox txtSite1;
        private System.Windows.Forms.TextBox txtSite2;
    }
}

