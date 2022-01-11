
namespace MailSlotsClient
{
    partial class Form1
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
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.connectBtn = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.serverList = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(322, 91);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "логин";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(373, 91);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(223, 22);
            this.textBox1.TabIndex = 1;
            this.textBox1.Text = "arseniy";
            // 
            // connectBtn
            // 
            this.connectBtn.Location = new System.Drawing.Point(325, 61);
            this.connectBtn.Name = "connectBtn";
            this.connectBtn.Size = new System.Drawing.Size(75, 23);
            this.connectBtn.TabIndex = 2;
            this.connectBtn.Text = "connect";
            this.connectBtn.UseVisualStyleBackColor = true;
            this.connectBtn.Click += new System.EventHandler(this.connectBtn_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(46, 61);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(123, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "Найти сервера";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // serverList
            // 
            this.serverList.FormattingEnabled = true;
            this.serverList.ItemHeight = 16;
            this.serverList.Location = new System.Drawing.Point(49, 113);
            this.serverList.Name = "serverList";
            this.serverList.Size = new System.Drawing.Size(223, 84);
            this.serverList.TabIndex = 6;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(608, 286);
            this.Controls.Add(this.serverList);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.connectBtn);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button connectBtn;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox serverList;
    }
}