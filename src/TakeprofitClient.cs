using System.Text;
using System.Net.Sockets;

namespace TakeprofitTechnologyTestTask.src
{
    public class TakeprofitClient
    {
        static Char StopSymbol = '\n';
        int MaxAttempt;
        TcpClient Client;
        NetworkStream Stream;
        int Port;
        String Address;
        string? Message = null;
        NumberGenerator Generator;
        NumbersStorage Storage;

        public TakeprofitClient(
            NumberGenerator numberGenerator,
            NumbersStorage numbersStorage,
            string address = "88.212.241.115",
            int port = 2013,
            int maxAttempt = 5)
        {
            Generator = numberGenerator;
            Storage = numbersStorage;
            Address = address;
            Port = port;
            MaxAttempt = maxAttempt;
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
                    throw;
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
            Storage.AddNumber(intResult);
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
    }
}