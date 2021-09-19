using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Messenger.MVVM.ViewModel;

namespace Messenger.Core.Utils
{
    internal class ServerConnect
    {
        private static string userName;
        private const string host = "127.0.0.1";
        private const int port = 8888;
        private static TcpClient client;
        private static NetworkStream stream;

        public void ConnectWithTcpServer()
        {
            userName = MainViewModel.User.Username;
            client = new TcpClient();
            try
            {
                client.Connect(host, port); //подключение клиента
                stream = client.GetStream(); // получаем поток
                
                // string message = userName;
                // byte[] data = Encoding.Unicode.GetBytes(message);
                // stream.Write(data, 0, data.Length);

                // запускаем новый поток для получения данных
                Thread receiveThread = new Thread(new ThreadStart(ReceiveMessage));
                receiveThread.Start(); //старт потока
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // отправка сообщений
        public void SendMessage(string message)
        {
            byte[] data = Encoding.Unicode.GetBytes(message);
            stream.Write(data, 0, data.Length);
        }

        // получение сообщений
        static void ReceiveMessage()
        {
            try
            {
                byte[] data = new byte[1024]; // буфер для получаемых данных
                StringBuilder builder = new StringBuilder();
                int bytes = 0;
                do
                {
                    bytes = stream.Read(data, 0, data.Length);
                    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                } while (stream.DataAvailable);

                string message = builder.ToString();
                Console.WriteLine(message); //вывод сообщения
            }
            catch
            {
                Console.WriteLine("Подключение прервано!"); //соединение было прервано
                Disconnect();
            }
        }

        static void Disconnect()
        {
            if (stream != null)
                stream.Close(); //отключение потока
            if (client != null)
                client.Close(); //отключение клиента
            Environment.Exit(0); //завершение процесса
        }
    }
}