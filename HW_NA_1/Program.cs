﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Server("Hello");
        }

        static void tasj1()
        {
            Message msg = new Message() { Text = "Hello", DateTime = DateTime.Now, NicknameFrom = "Ivan", NicknameTo = "All" };
            string json = msg.SerializeMessageToJson();
            Console.WriteLine(json);
            Message? msgDeserialized = Message.DeserializeFromJson(json);
        }

        public static void Server(string message)
        { 
            UdpClient udpClient = new UdpClient(12345);
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, 0);
            Console.WriteLine("Сервер ждет сообщение от клиента.Нажмите любую клавишу для выхода.");
            
            while (!Console.KeyAvailable) // Ждем нажатия любой клавиши
                {
                byte[] buffer = udpClient.Receive(ref iPEndPoint);

                if (buffer == null) break;
                var messageText = Encoding.UTF8.GetString(buffer);

                Message message1 = Message.DeserializeFromJson(messageText);
                message1.Print();

                // Отправляем подтверждение клиенту
                string confirmationMessage = "Сообщение успешно обработано на сервере";
                byte[] confirmationBuffer = Encoding.UTF8.GetBytes(confirmationMessage);
                udpClient.Send(confirmationBuffer, confirmationBuffer.Length, iPEndPoint);
                Console.WriteLine(confirmationMessage);
            }

            Console.WriteLine("Сервер завершил работу.");
            udpClient.Close();

        }
    }
}