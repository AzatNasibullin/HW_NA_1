using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static async Task Main(string[] args)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            
            Task task = Task.Run(() => NetServer.Server("Hello", cts));

            Console.WriteLine("Для выхода нажмите любую клавишу...");
            Console.ReadKey();
            cts.Cancel();

            await task;
            await task1();
            await task2();
        }

        static async Task task1()
        {
            Message msg = new Message() { Text = "Hello", DateTime = DateTime.Now, NicknameFrom = "Ivan", NicknameTo = "All" };
            string json = msg.SerializeMessageToJson();
            Console.WriteLine(json);
            Message? msgDeserialized = await Task.Run(() => Message.DeserializeFromJson(json));
        }
        static async Task task2()
        {
            NetServer server = new NetServer();
            server.msg = "Hi ";
            server.SaveState(server.cts.Token);
            Task serverTask = NetServer.Server(server.msg, server.cts);
            server.RestoreState();
            await serverTask;

        }
    }
}


