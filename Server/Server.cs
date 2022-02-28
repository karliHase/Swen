using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    class Server
    {
        private TcpListener listener;
        private Boolean isRunning;

        public Server(int port)
        {
            listener = new TcpListener(IPAddress.Any, port);
            listener.Start(10);

            isRunning = true;

            AcceptClientsLoop();

        }

        public void Stop()
        {
            isRunning = false;
        }

        public void AcceptClientsLoop()
        {
            try
            {
                while (isRunning)
                {
                    //wait for client connection 
                    TcpClient tcpClient = server.AcceptTcpClient();

                    //create new thread to handle communication
                    Thread clientThread = new Thread(HandleClient);
                    clientThread.Start(tcpClient);
                }
            }
            catch(Exception e)
            {
                string failed = "Accepting TcpClient failed: " + e.Message;
                Console.WriteLine(failed);

            }
           
        }

        private void HandleClient()
        {

        }

    }
}
