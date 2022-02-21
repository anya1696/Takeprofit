namespace TakeprofitTechnologyTestTask.src
{
    public class Program
    {
        static List<Thread> threads= new List<Thread>();
        static void Main()
        {
            for (int i = 0; i < 15; i++)
            {
                Thread requestThread = new Thread(RequestNumbers);
                threads.Add(requestThread);
                requestThread.Start();
            }

            WaitUntilAllThreadsComplete();
        }
        static void RequestNumbers()
        {
            TakeprofitClient client = new TakeprofitClient();
            client.Start();
        }

        static void WaitUntilAllThreadsComplete()
        {
            foreach (Thread machineThread in threads)
            {
                machineThread.Join();
            }
            NumbersStorage.GetMedian();
        }
    }
}