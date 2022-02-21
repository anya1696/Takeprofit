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
        Char StopSymbol = '\n';

        int MaxAttempt = 5;
        
        // TODO: сделать локальной переменной
        string? Message = null;

        public void Start()
        {
            Connect();
            do
            {
                Message = GenerateMessage();
                if (Message == null) continue;
                Iteration(Message);
            } while (Message != null);
            CloseConnection();
        }

        void Iteration(string? message, int attempt = 0)
        {
            if (message == null){
                return;
            }
            try
            {
                Send(message);
                HandleResponse();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0} | {1}", e, attempt);
                if (attempt >= MaxAttempt)
                {
                    return;
                    //throw;
                }
                Connect();
                Console.WriteLine("Reconnect");
                Iteration(message, attempt + 1);
            }
        }

        void Connect()
        {
            Client = new TcpClient(Address, Port);
            Stream = Client.GetStream();
        }

        string? GenerateMessage()
        {
            if (!NumberGenerator.IsFinished())
            {
                SendNumberToStorage();
                return null;
            }
            int number = NumberGenerator.Next();
            return number.ToString() + "\n";
        }
        void Send(string message)
        {
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
            Stream.Write(data, 0, data.Length);
            Console.Write("Sent: {0} ", message);
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
                if (responseSize == 0)
                {
                    throw new Exception();
                }
                myCompleteMessage.Append(response);
            }
            while (!myCompleteMessage.ToString().Contains(StopSymbol));
            int intResult = ParseNumber(myCompleteMessage.ToString());
            Results.Add(intResult);
            Console.WriteLine("Received: {0}, Seded: {1}", intResult, Message);
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

        void SendNumberToStorage(){
            NumbersStorage.AddNumbers(Results);
        }
    }
}