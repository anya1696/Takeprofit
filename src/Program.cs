namespace TakeprofitTechnologyTestTask.src
{
    public class Program
    {
        static List<Thread> threads = new List<Thread>();
        static int MinNumber = 1;
        static int MaxNumber = 2018;
        
        static void Main()
        {
            NumberGenerator generator = new NumberGenerator(MinNumber, MaxNumber);
            NumbersStorage storage = new NumbersStorage();

            for (int i = 0; i < Environment.ProcessorCount; i++)
            {
                Thread requestThread = new Thread(() => RequestNumbers(generator, storage));
                threads.Add(requestThread);
                requestThread.Start();
            }

            WaitUntilAllThreadsComplete();
            Console.Write("Median number: {0}", storage.GetMedian());
        }
        static void RequestNumbers(NumberGenerator generator, NumbersStorage storage)
        {
            TakeprofitClient client = new TakeprofitClient(generator, storage);
            client.Start();
        }

        static void WaitUntilAllThreadsComplete()
        {
            foreach (Thread machineThread in threads)
            {
                machineThread.Join();
            }
        }
    }
}