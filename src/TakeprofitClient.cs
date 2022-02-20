using System.Text;
using System.Net.Sockets;
using System;

namespace TakeprofitTechnologyTestTask.src
{
    public class TakeprofitClient
    {
        int Port = 2013;
        String Address = "88.212.241.115";
        TcpClient Client;
        NetworkStream Stream;
        List<int> Results = new List<int>();

        public TakeprofitClient()
        {
            Client = new TcpClient(Address, Port);
            Stream = Client.GetStream();
        }

        public void Start()
        {
            string message = GenerateMessage();
            if (message == String.Empty)
            {
                return;
            }
            Send(message);
            HandleResponse();
        }

        string GenerateMessage()
        {
            if (NumberGenerator.IsFinished())
            {
                return String.Empty;
            }
            int number = NumberGenerator.Next();
            return number.ToString() + "\n";
        }
        void Send(string message)
        {
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
            Stream.Write(data, 0, data.Length);
            Console.Write("Sent: {0}", message);
        }

        void CloseConnection()
        {
            Stream.Close();
            Client.Close();
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
                if (responseSize > 0)
                {
                    myCompleteMessage.Append(response);
                    Console.Write(response);
                }
            }
            while (!myCompleteMessage.ToString().Contains("\n") || Stream.DataAvailable);
            Console.WriteLine("Received:{0}", myCompleteMessage);
            Console.WriteLine("Parsed:{0}", ParseNumber(myCompleteMessage.ToString()));
        }

        int ParseNumber(string str)
        {
            int res = 0;
            StringBuilder strRes = new StringBuilder();
            foreach (char c in str)
            {
                if (c >= '0' && c <= '9')
                {
                    strRes.Append(c);
                }
            }
            res = Int32.Parse(strRes.ToString());
            return res;
        }
    }
}