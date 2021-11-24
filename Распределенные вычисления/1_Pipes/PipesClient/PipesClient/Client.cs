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
using PipesClient;

namespace Pipes
{
    public partial class frmMain : Form
    {
        private Int32 PipeHandle;   // дескриптор канала
        private Thread t;                                                               // поток для обслуживания канала
        private bool _continue = true;                                                  // флаг, указывающий продолжается ли работа с каналом
        public static string CHANNEL_NAME = "";
        // конструктор формы
        public frmMain()
        {
            Form1 form = new Form1();
            var result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                CHANNEL_NAME = form.login;
            }
            InitializeComponent();
            loginLabel.Text = CHANNEL_NAME;
            
            this.Text += "     " + Dns.GetHostName();   // выводим имя текущей машины в заголовок формы
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (messageBox.Text == "")
            {
                MessageBox.Show("Введите сообщение");
                return;
            }
            SendMessage(serverBox.Text, CHANNEL_NAME, messageBox.Text);
            messageBox.Text = "";
        }

        private void MessageListener()
        {
            string msg = "";            // прочитанное сообщение
            uint realBytesReaded = 0;   // количество реально прочитанных из канала байтов

            // входим в бесконечный цикл работы с каналом
            while (_continue)
            {
                if (DIS.Import.ConnectNamedPipe(PipeHandle, 0))
                {
                    byte[] buff = new byte[1024];                                           // буфер прочитанных из канала байтов
                    DIS.Import.FlushFileBuffers(PipeHandle);                                // "принудительная" запись данных, расположенные в буфере операционной системы, в файл именованного канала
                    DIS.Import.ReadFile(PipeHandle, buff, 1024, ref realBytesReaded, 0);    // считываем последовательность байтов из канала в буфер buff
                    msg = Encoding.Unicode.GetString(buff);                                 // выполняем преобразование байтов в последовательность символов
                    recieveBox.Invoke((MethodInvoker)delegate
                    {
                        if (msg != "")
                            recieveBox.Text += "\n >> " + msg;                             // выводим полученное сообщение на форму
                    });

                    DIS.Import.DisconnectNamedPipe(PipeHandle);                             // отключаемся от канала клиента 
                    Thread.Sleep(500);                                                      // приостанавливаем работу потока перед тем, как приcтупить к обслуживанию очередного клиента
                }
            }
        }

        private void SendMessage(string channelName, string login, string message)
        {
            uint BytesWritten = 0;  // количество реально записанных в канал байт
            byte[] buff = Encoding.Unicode.GetBytes(login + " >> " + message);    // выполняем преобразование сообщения (вместе с идентификатором машины) в последовательность байт

            // открываем именованный канал, имя которого указано в поле tbPipe
            int pipeSender = DIS.Import.CreateFile(channelName, DIS.Types.EFileAccess.GenericWrite, DIS.Types.EFileShare.Read, 0, DIS.Types.ECreationDisposition.OpenExisting, 0, 0);
            DIS.Import.WriteFile(pipeSender, buff, Convert.ToUInt32(buff.Length), ref BytesWritten, 0);         // выполняем запись последовательности байт в канал
            DIS.Import.CloseHandle(pipeSender);              // закрываем дескриптор канала
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            _continue = false;      // сообщаем, что работа с каналом завершена

            if (t != null)
                t.Abort();          // завершаем поток

            if (PipeHandle != -1)
                DIS.Import.CloseHandle(PipeHandle);     // закрываем дескриптор канала
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            SendMessage(serverBox.Text, CHANNEL_NAME, CHANNEL_NAME + " зашел пообщаться в чат");
            // создание именованного канала
            PipeHandle = DIS.Import.CreateNamedPipe("\\\\.\\pipe\\" + CHANNEL_NAME, DIS.Types.PIPE_ACCESS_DUPLEX, DIS.Types.PIPE_TYPE_BYTE | DIS.Types.PIPE_WAIT, DIS.Types.PIPE_UNLIMITED_INSTANCES, 0, 1024, DIS.Types.NMPWAIT_WAIT_FOREVER, (uint)0);
            // создание потока, отвечающего за работу с каналом
            t = new Thread(MessageListener);
            t.Start();
        }
    }
}
