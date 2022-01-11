using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Text;
using System.Windows.Forms;

namespace MailSlotsClient
{
    public partial class Form1 : Form
    {
        public string login;
        public string mailslotName;
        private int ClientHandleMailslot;       // дескриптор мэйлслота
        private Thread t;                                                               // поток для обслуживания канала
        private bool _continue = true;
        private List<string> avaliableServers = new List<string>();

        public Form1()
        {
            InitializeComponent();
            ClientHandleMailslot = DIS.Import.CreateMailslot("\\\\.\\mailslot\\ping", 0, DIS.Types.MAILSLOT_WAIT_FOREVER, 0);

            t = new Thread(MessageListener);
            t.Start();

        }
        private void connectBtn_Click(object sender, EventArgs e)
        {
            
            this.login = textBox1.Text;

            if (serverList.SelectedItem != null)
            {
                string serverName = serverList.SelectedItem.ToString();
                string output = new string(serverName.Where(c => !char.IsControl(c)).ToArray());
                this.mailslotName = "\\\\" + output + "\\mailslot\\ServerMailslot";
                this.DialogResult = DialogResult.OK;

            }
            else
            {
                MessageBox.Show("Select server from the list");

            }


        }
        private void AddServer(string server)
        {
            if (!avaliableServers.Contains(server))
            {
                avaliableServers.Add(server);
                serverList.Invoke((MethodInvoker)delegate
                {
                    serverList.Items.Clear();
                    serverList.Items.AddRange(avaliableServers.ToArray());

                });

            }
        }
        private void serverBox_TextChanged(object sender, EventArgs e)
        {

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

                        AddServer(msg);
                        Thread.Sleep(500);                                      // приостанавливаем работу потока перед тем, как приcтупить к обслуживанию очередного клиента
                    }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            uint BytesWritten = 0;  // количество реально записанных в канал байт
            byte[] buff = Encoding.Unicode.GetBytes("ping >> ping");    // выполняем преобразование сообщения (вместе с идентификатором машины) в последовательность байт

            // открываем именованный канал, имя которого указано в поле tbPipe
            int mailslotsSender = DIS.Import.CreateFile("\\\\*\\mailslot\\ServerMailslot", DIS.Types.EFileAccess.GenericWrite, DIS.Types.EFileShare.Read, 0, DIS.Types.ECreationDisposition.OpenExisting, 0, 0);
            DIS.Import.WriteFile(mailslotsSender, buff, Convert.ToUInt32(buff.Length), ref BytesWritten, 0);         // выполняем запись последовательности байт в канал
            DIS.Import.CloseHandle(mailslotsSender);              // закрываем дескриптор канала


        }
    }
}
