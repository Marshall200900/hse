namespace MailSlots
{
    partial class frmMain
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnSend = new System.Windows.Forms.Button();
            this.messageBox = new System.Windows.Forms.TextBox();
            this.rtbMessage = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.loginLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(381, 428);
            this.btnSend.Margin = new System.Windows.Forms.Padding(4);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(121, 32);
            this.btnSend.TabIndex = 3;
            this.btnSend.Text = "Отправить";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // messageBox
            // 
            this.messageBox.Location = new System.Drawing.Point(131, 433);
            this.messageBox.Margin = new System.Windows.Forms.Padding(4);
            this.messageBox.Name = "messageBox";
            this.messageBox.Size = new System.Drawing.Size(240, 22);
            this.messageBox.TabIndex = 1;
            // 
            // rtbMessage
            // 
            this.rtbMessage.Location = new System.Drawing.Point(19, 64);
            this.rtbMessage.Name = "rtbMessage";
            this.rtbMessage.Size = new System.Drawing.Size(594, 357);
            this.rtbMessage.TabIndex = 4;
            this.rtbMessage.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(441, 16);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "Логин:";
            // 
            // loginLabel
            // 
            this.loginLabel.AutoSize = true;
            this.loginLabel.Location = new System.Drawing.Point(496, 16);
            this.loginLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.loginLabel.Name = "loginLabel";
            this.loginLabel.Size = new System.Drawing.Size(38, 17);
            this.loginLabel.TabIndex = 6;
            this.loginLabel.Text = "login";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(625, 473);
            this.Controls.Add(this.loginLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rtbMessage);
            this.Controls.Add(this.messageBox);
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
        private System.Windows.Forms.TextBox messageBox;
        private System.Windows.Forms.RichTextBox rtbMessage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label loginLabel;
    }
}

