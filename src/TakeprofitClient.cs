using System.Text;
using System.Net.Sockets;

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
        NumberGenerator Generator;
        NumbersStorage Storage;

        public TakeprofitClient(NumberGenerator numberGenerator, NumbersStorage numbersStorage)
        {
            Generator = numberGenerator;
            Storage = numbersStorage;
        }

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

        void Iteration(string message, int attempt = 0)
        {
            try
            {
                Send(message);
                HandleResponse();
            }
            catch (Exception e)
            {
                if (!(e is ConnectionClosedExeption ||
                      e is SocketException))
                {
                    throw;
                }

                Console.WriteLine("Exception: {0} | {1}", e, attempt);
                if (attempt >= MaxAttempt)
                {
                    return;
                }
                CloseConnection();
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
            if (!Generator.HasNext())
            {
                SendNumberToStorage();
                return null;
            }
            int number = Generator.Next();
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
            StringBuilder completeMessage = new StringBuilder();
            string response;
            do
            {
                responseSize = Stream.Read(buffer, 0, buffer.Length);
                response = Encoding.ASCII.GetString(buffer, 0, responseSize);
                if (responseSize == 0)
                {
                    throw new ConnectionClosedExeption();
                }
                completeMessage.Append(response);
            }
            while (!response.Contains(StopSymbol));
            int intResult = ParseNumber(completeMessage.ToString());
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

        void SendNumberToStorage()
        {
            Storage.AddNumbers(Results);
        }
    }
}