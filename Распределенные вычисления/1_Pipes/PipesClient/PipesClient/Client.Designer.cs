namespace Pipes
{
    partial class frmMain
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
            this.btnSend = new System.Windows.Forms.Button();
            this.lblPipe = new System.Windows.Forms.Label();
            this.serverBox = new System.Windows.Forms.TextBox();
            this.messageBox = new System.Windows.Forms.TextBox();
            this.recieveBox = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.loginLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(387, 366);
            this.btnSend.Margin = new System.Windows.Forms.Padding(4);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(100, 28);
            this.btnSend.TabIndex = 2;
            this.btnSend.Text = "Отправить";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // lblPipe
            // 
            this.lblPipe.AutoSize = true;
            this.lblPipe.Location = new System.Drawing.Point(16, 11);
            this.lblPipe.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPipe.Name = "lblPipe";
            this.lblPipe.Size = new System.Drawing.Size(92, 34);
            this.lblPipe.TabIndex = 1;
            this.lblPipe.Text = "Введите имя\r\nканала";
            // 
            // serverBox
            // 
            this.serverBox.Location = new System.Drawing.Point(120, 18);
            this.serverBox.Margin = new System.Windows.Forms.Padding(4);
            this.serverBox.Name = "serverBox";
            this.serverBox.Size = new System.Drawing.Size(132, 22);
            this.serverBox.TabIndex = 0;
            this.serverBox.Text = "\\\\.\\pipe\\ServerPipe";
            // 
            // messageBox
            // 
            this.messageBox.Location = new System.Drawing.Point(19, 369);
            this.messageBox.Margin = new System.Windows.Forms.Padding(4);
            this.messageBox.Name = "messageBox";
            this.messageBox.Size = new System.Drawing.Size(350, 22);
            this.messageBox.TabIndex = 1;
            // 
            // recieveBox
            // 
            this.recieveBox.Location = new System.Drawing.Point(19, 48);
            this.recieveBox.Name = "recieveBox";
            this.recieveBox.Size = new System.Drawing.Size(468, 311);
            this.recieveBox.TabIndex = 3;
            this.recieveBox.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(260, 21);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 17);
            this.label1.TabIndex = 4;
            this.label1.Text = "Логин";
            // 
            // loginLabel
            // 
            this.loginLabel.AutoSize = true;
            this.loginLabel.Location = new System.Drawing.Point(315, 21);
            this.loginLabel.Name = "loginLabel";
            this.loginLabel.Size = new System.Drawing.Size(73, 17);
            this.loginLabel.TabIndex = 5;
            this.loginLabel.Text = "loginLabel";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(511, 416);
            this.Controls.Add(this.loginLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.recieveBox);
            this.Controls.Add(this.messageBox);
            this.Controls.Add(this.serverBox);
            this.Controls.Add(this.lblPipe);
            this.Controls.Add(this.btnSend);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Клиент";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Label lblPipe;
        private System.Windows.Forms.TextBox serverBox;
        private System.Windows.Forms.TextBox messageBox;
        private System.Windows.Forms.RichTextBox recieveBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label loginLabel;
    }
}