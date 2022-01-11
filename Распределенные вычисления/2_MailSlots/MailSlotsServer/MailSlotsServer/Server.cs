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
using System.Threading;
using System.Runtime.InteropServices;

namespace MailSlots
{
    public partial class frmMain : Form
    {
        private int ClientHandleMailSlot;       // дескриптор мэйлслота
        private string MailSlotName = "\\\\" + Dns.GetHostName() + "\\mailslot\\ServerMailslot";    // имя мэйлслота, Dns.GetHostName() - метод, возвращающий имя машины, на которой запущено приложение
        private Thread t;                       // поток для обслуживания мэйлслота
        private bool _continue = true;          // флаг, указывающий продолжается ли работа с мэйлслотом
                                                     // дескриптор канала

        private List<string> connectedUsers = new List<string>();

        // конструктор формы
        public frmMain()
        {
            InitializeComponent();

            // создание мэйлслота
            ClientHandleMailSlot = DIS.Import.CreateMailslot("\\\\.\\mailslot\\ServerMailslot", 0, DIS.Types.MAILSLOT_WAIT_FOREVER, 0);

            // вывод имени мэйлслота в заголовок формы, чтобы можно было его использовать для ввода имени в форме клиента, запущенного на другом вычислительном узле
            this.Text += "     " + MailSlotName;

            // создание потока, отвечающего за работу с мэйлслотом
            Thread t = new Thread(ReceiveMessage);
            t.Start();
        }

        private void ReceiveMessage()
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
                DIS.Import.GetMailslotInfo(ClientHandleMailSlot, MailslotSize, ref lpNextSize, ref MessageCount, 0);

                // если есть сообщения в мэйлслоте, то обрабатываем каждое из них
                if (MessageCount > 0)
                    for (int i = 0; i < MessageCount; i++)
                    {
                        byte[] buff = new byte[1024];                           // буфер прочитанных из мэйлслота байтов
                        DIS.Import.FlushFileBuffers(ClientHandleMailSlot);      // "принудительная" запись данных, расположенные в буфере операционной системы, в файл мэйлслота
                        DIS.Import.ReadFile(ClientHandleMailSlot, buff, 1024, ref realBytesReaded, 0);      // считываем последовательность байтов из мэйлслота в буфер buff
                        msg = Encoding.Unicode.GetString(buff);                 // выполняем преобразование байтов в последовательность символов
                        string[] usernameMessage = msg.Split(new string[] { " >> " }, StringSplitOptions.None);
                        if (usernameMessage[1].Contains("ping"))
                        {
                            SendMessage(usernameMessage[0], Dns.GetHostName());
                            break;
                        }
                        rtbMessages.Invoke((MethodInvoker)delegate
                        {
                            if (msg != "" && !usernameMessage[1].Contains("ping"))
                            {
                                rtbMessages.Text += "\n >> " + msg + " \n";          // выводим полученное сообщение на форму
                            }
                          
                        });
                        AddUserIfNeedTo(usernameMessage[0]);
                        PingClients(msg);
                        Thread.Sleep(500);                                      // приостанавливаем работу потока перед тем, как приcтупить к обслуживанию очередного клиента
                    }
            }
        }

        private void AddUserIfNeedTo(string user)
        {
            if (!connectedUsers.Contains(user))
            {
                connectedUsers.Add(user);
            }
        }
        private void PingClients(string msg)
        {
            foreach (string user in connectedUsers)
            {
                SendMessage(user, msg);
                Thread.Sleep(500); // магический костыль
            }
        }
        private void SendMessage(string username, string message)
        {
            string name = "\\\\.\\mailslot\\" + username;
            uint BytesWritten = 0;  // количество реально записанных в канал байт
            byte[] buff = Encoding.Unicode.GetBytes(message);    // выполняем преобразование сообщения (вместе с идентификатором машины) в последовательность байт

            // открываем именованный канал, имя которого указано в поле tbPipe
            int mailslotsSender = DIS.Import.CreateFile(name, DIS.Types.EFileAccess.GenericWrite, DIS.Types.EFileShare.Read, 0, DIS.Types.ECreationDisposition.OpenExisting, 0, 0);
            DIS.Import.WriteFile(mailslotsSender, buff, Convert.ToUInt32(buff.Length), ref BytesWritten, 0);         // выполняем запись последовательности байт в канал
            DIS.Import.CloseHandle(mailslotsSender);              // закрываем дескриптор канала
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            _continue = false;      // сообщаем, что работа с мэйлслотом завершена

            if (t != null)
                t.Abort();          // завершаем поток

            if (ClientHandleMailSlot != -1)
                DIS.Import.CloseHandle(ClientHandleMailSlot);            // закрываем дескриптор мэйлслота
        }

    }
}