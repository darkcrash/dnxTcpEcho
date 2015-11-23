using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace dnxTcpEcho
{
    public class Program
    {

        private Socket sock;

        public void Main(string[] args)
        {
            this.InitSocket();
            try
            {
                while (true)
                {
                    var client = sock.Accept();
                    var t = new Task(() => this.InitSocketClient(client), TaskCreationOptions.LongRunning);
                    t.Start(TaskScheduler.Default);
                }
            }
            finally
            {
                sock.Dispose();
            }
        }


        private void InitSocket()
        {
            this.sock = new Socket(SocketType.Stream, ProtocolType.Tcp);
            var endp = new System.Net.IPEndPoint(System.Net.IPAddress.Any, 5999);
            sock.Bind(endp);
            sock.Listen(100);
        }


        private void InitSocketClient(Socket client)
        {
            byte[] buf = new byte[1024];

            while (true)
            {
                var size = client.Receive(buf);
                if (size == 0) break;
                client.Send(buf, size, SocketFlags.None);
            }

        }

    }
}
