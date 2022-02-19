using System.Text;
using System.Net.Sockets;
using System;

namespace TakeprofitTechnologyTestTask.src
{
    public class TakeprofitClient
    {
        int Port = 2013;
        String Address = "88.212.241.115";
        String Message = "87\n";
        TcpClient Client;
        NetworkStream Stream;

        public TakeprofitClient()
        {
            Client = new TcpClient(Address, Port);
            Stream = Client.GetStream();
        }

        public void Start()
        {
            Send();
            HandleResponse();
            Stream.Close();
            Client.Close();
        }

        void Send()
        {
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(Message);
            Stream.Write(data, 0, data.Length);
            Console.Write("Sent: {0}", Message);
        }

        void HandleResponse()
        {
            Byte[] buffer = new byte[256];
            Int32 responseSize;
            StringBuilder myCompleteMessage = new StringBuilder();
            do
            {
                responseSize = Stream.Read(buffer, 0, buffer.Length);
                string response = Encoding.ASCII.GetString(buffer, 0, responseSize);
                if (responseSize > 0){
                    myCompleteMessage.Append(response);
                    Console.Write(response);
                }
            }
            while (!myCompleteMessage.ToString().Contains("\n") || Stream.DataAvailable);
            Console.WriteLine("Received:{0}", myCompleteMessage);
        }        
    }
}