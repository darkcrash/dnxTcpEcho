﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace dnxTcpEcho
{
    public class Program
    {

        private static Socket server;
        private static List<Socket> clientList = new List<Socket>();
        private static object clientListLock = new object();
        private static CancellationTokenSource source = new CancellationTokenSource();
        private static CancellationToken token = source.Token;

        public static void Main(string[] args)
        {
            InitSocket();
            while (true)
            {
                var inp = Console.ReadKey(true);
                if (inp.Key == ConsoleKey.Escape) break;
                var data = System.Text.Encoding.ASCII.GetBytes(inp.KeyChar.ToString());
                var result = Parallel.ForEach(clientList, async (client) => await SocketTaskExtensions.SendAsync(client, new ArraySegment<byte>(data), SocketFlags.None));
            }
            Console.WriteLine("Shutdown");
            source.Cancel(true);
            var resultShutdown = Parallel.ForEach(clientList, (client) => client.Shutdown(SocketShutdown.Both));
        }


        private static void InitSocket()
        {
            server = new Socket(SocketType.Stream, ProtocolType.Tcp);
            var endp = new System.Net.IPEndPoint(System.Net.IPAddress.Any, 5999);
            server.Bind(endp);
            server.Listen(100);
            Console.WriteLine($"{nameof(InitSocket)} {endp}");

            Action loopAccept = async () =>
            {
                while (true)
                {
                    var client = await server.AcceptAsync();
                    var t = new Task(_ => InitSocketClient(client), token, TaskCreationOptions.LongRunning);
                    t.Start();
                }
            };
            var loopAcceptTask = new Task(loopAccept, token, TaskCreationOptions.LongRunning);
            loopAcceptTask.Start();


        }


        private static async void InitSocketClient(Socket client)
        {
            lock (clientListLock)
                clientList.Add(client);
            var endp = client.RemoteEndPoint.ToString();
            Console.WriteLine($"Accept Client {endp}");
            byte[] buf = new byte[1024];

            while (true)
            {
                var size = await client.ReceiveAsync(new ArraySegment<byte>(buf), SocketFlags.None);
                if (size <= 0) break;
                await client.SendAsync(new ArraySegment<byte>(buf, 0, size), SocketFlags.None);
            }
            lock (clientListLock)
                clientList.Remove(client);
            Console.WriteLine($"Close Client {endp}");
        }

    }
}
