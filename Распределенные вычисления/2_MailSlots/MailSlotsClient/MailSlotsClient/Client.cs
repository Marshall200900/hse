using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using MailSlotsClient;


namespace MailSlots
{
    public partial class frmMain : Form
    {
        private int ClientHandleMailslot;       // дескриптор мэйлслота
        private Thread t;                                                               // поток для обслуживания канала
        private bool _continue = true;                                                  // флаг, указывающий продолжается ли работа с каналом
        public static string CHANNEL_NAME = "";
        public static string mailslotName;
        // конструктор формы
        public frmMain()
        {
            Form1 form = new Form1();
            var result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                CHANNEL_NAME = form.login;
                mailslotName = form.mailslotName;
            }
            InitializeComponent();
            loginLabel.Text = CHANNEL_NAME;

            this.Text += "     " + Dns.GetHostName();   // выводим имя текущей машины в заголовок формы
        }


        // отправка сообщения
        private void btnSend_Click(object sender, EventArgs e)
        {
            if (messageBox.Text == "")
            {
                MessageBox.Show("Введите сообщение");
                return;
            }
            SendMessage(mailslotName, CHANNEL_NAME, messageBox.Text);     // выполняем запись последовательности байт в мэйлслот
        }

        private void MessageListener()
        {
            string msg = "";            // прочитанное сообщение
            int MailslotSize = 0;       // максимальный размер сообщения
            int lpNextSize = 0;         // размер следующего сообщения
            int MessageCount = 0;       // количество сообщений в мэйлслоте
            uint realBytesReaded = 0;   // количество реально прочитанных из мэйлслота байтов

            // входим в бесконечный цикл работы с мэйлслотом
            while (_continue)
            {
                // получаем информацию о состоянии мэйлслота
                DIS.Import.GetMailslotInfo(ClientHandleMailslot, MailslotSize, ref lpNextSize, ref MessageCount, 0);

                // если есть сообщения в мэйлслоте, то обрабатываем каждое из них
                if (MessageCount > 0)
                    for (int i = 0; i < MessageCount; i++)
                    {
                        byte[] buff = new byte[1024];                           // буфер прочитанных из мэйлслота байтов
                        DIS.Import.FlushFileBuffers(ClientHandleMailslot);      // "принудительная" запись данных, расположенные в буфере операционной системы, в файл мэйлслота
                        DIS.Import.ReadFile(ClientHandleMailslot, buff, 1024, ref realBytesReaded, 0);      // считываем последовательность байтов из мэйлслота в буфер buff
                        msg = Encoding.Unicode.GetString(buff);                 // выполняем преобразование байтов в последовательность символов

                        rtbMessage.Invoke((MethodInvoker)delegate
                        {
                            if (msg != "")
                                rtbMessage.Text += "\n >> " + msg + " \n";     // выводим полученное сообщение на форму
                        });
                        Thread.Sleep(500);                                      // приостанавливаем работу потока перед тем, как приcтупить к обслуживанию очередного клиента
                    }
            }
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            DIS.Import.CloseHandle(ClientHandleMailslot);     // закрываем дескриптор мэйлслота
        }
        private void SendMessage(string mailslotName, string login, string message)
        {
            uint BytesWritten = 0;  // количество реально записанных в канал байт
            byte[] buff = Encoding.Unicode.GetBytes(login + " >> " + message);    // выполняем преобразование сообщения (вместе с идентификатором машины) в последовательность байт

            // открываем именованный канал, имя которого указано в поле tbPipe
            int mailslotsSender = DIS.Import.CreateFile(mailslotName, DIS.Types.EFileAccess.GenericWrite, DIS.Types.EFileShare.Read, 0, DIS.Types.ECreationDisposition.OpenExisting, 0, 0);
            DIS.Import.WriteFile(mailslotsSender, buff, Convert.ToUInt32(buff.Length), ref BytesWritten, 0);         // выполняем запись последовательности байт в канал
            DIS.Import.CloseHandle(mailslotsSender);              // закрываем дескриптор канала
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            SendMessage(mailslotName, CHANNEL_NAME, CHANNEL_NAME + " зашел в сеть");
            ClientHandleMailslot = DIS.Import.CreateMailslot("\\\\.\\mailslot\\" + CHANNEL_NAME, 0, DIS.Types.MAILSLOT_WAIT_FOREVER, 0);
            // создание потока, отвечающего за работу с каналом
            t = new Thread(MessageListener);
            t.Start();
        }
    }
}