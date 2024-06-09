﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{
    class Program
    {
        static bool exit = false;
        static void Main(string[] args)
        {
            CancellationTokenSource cts = new CancellationTokenSource();

            ThreadPool.QueueUserWorkItem(obj =>
            {
                NetServer.Server("Hello", cts);
            });

            Console.WriteLine("Для выхода нажмите любую клавишу...");
            Console.ReadKey();
            cts.Cancel();
        }

        static void tasj1()
        {
            Message msg = new Message() { Text = "Hello", DateTime = DateTime.Now, NicknameFrom = "Ivan", NicknameTo = "All" };
            string json = msg.SerializeMessageToJson();
            Console.WriteLine(json);
            Message? msgDeserialized = Message.DeserializeFromJson(json);
        }

    }
}


