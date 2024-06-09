using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class NetServer
    {

        //static bool exit = false;
        public static void Server(string message, CancellationTokenSource cancellationToken)
        {
            UdpClient udpClient = new UdpClient(12345);
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, 0);

            Console.WriteLine("Сервер ждет сообщение от клиента.Нажмите любую клавишу для выхода.");
            while (!cancellationToken.IsCancellationRequested)
            {
                byte[] buffer = udpClient.Receive(ref iPEndPoint);
                var messageText = Encoding.UTF8.GetString(buffer);

                ThreadPool.QueueUserWorkItem(obj =>
                {
                    Message message1 = Message.DeserializeFromJson(messageText);
                    message1.Print();

                    // Отправляем подтверждение клиенту
                    string confirmationMessage = "Сообщение успешно обработано на сервере";
                    byte[] confirmationBuffer = Encoding.UTF8.GetBytes(confirmationMessage);
                    udpClient.Send(confirmationBuffer, confirmationBuffer.Length, iPEndPoint);

                    if (message1.Text.ToLower() == "exit")
                    {
                        cancellationToken.Cancel();
                        Console.WriteLine("Завершение работы...");
                    }
                });
            }
            udpClient.Close();
        }
    }
}
