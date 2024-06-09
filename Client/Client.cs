using Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class NetClient
    {
        public static void SentMessage(string From, string ip)
        {
            UdpClient udpClient = new UdpClient();
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), 12345);


            while (true)
            {
                string messageText;
                do
                {
                    // Console.Clear();
                    Console.WriteLine("Введите сообщение (для выхода введите \"Exit\"): ");
                    messageText = Console.ReadLine();
                    if (messageText.ToLower() == "exit")
                    {
                        Console.WriteLine("Завершение работы клиента.");

                        udpClient.Close();
                        return;
                    }
                }
                while (string.IsNullOrEmpty(messageText));
                Message message = new Message() { Text = messageText, NicknameFrom = From, NicknameTo = "Server", DateTime = DateTime.Now };
                string json = message.SerializeMessageToJson();

                byte[] data = Encoding.UTF8.GetBytes(json);
                udpClient.Send(data, data.Length, ipEndPoint);

                // Получение подтверждения от сервера
                byte[] receivedBytes = udpClient.Receive(ref ipEndPoint);
                string confirmationMessage = Encoding.UTF8.GetString(receivedBytes);
                Console.WriteLine($"Подтверждение от сервера: {confirmationMessage}");

            }

        }
    }
}
