using System.Globalization;
namespace TakeprofitTechnologyTestTask.src
{
    public static class NumbersStorage
    {
        static List<int> Numbers = new List<int>();
        static Object loсker = new();
        public static void AddNumbers(List<int> numbersToAdd)
        {
            lock (loсker)
            {
                foreach (int nubber in numbersToAdd)
                {
                    Numbers.Add(nubber);
                }
            }
        }

        public static void GetMedian()
        {
            Numbers.Sort();
            double median;
            if (Numbers.Count % 2 == 0)
            {
                median = (Numbers[Numbers.Count / 2] + Numbers[Numbers.Count / 2 - 1]) / 2.0;
            }
            else
            {
                median = Numbers[(int)Math.Ceiling(Numbers.Count / 2.0)];
            }
            Console.Write("Median number: {0}", median);
        }
    }
}