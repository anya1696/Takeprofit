namespace TakeprofitTechnologyTestTask.src
{
    public class NumbersStorage
    {
        List<int> Numbers = new List<int>();
        Object loсker = new();
        public void AddNumbers(List<int> numbersToAdd)
        {
            lock (loсker)
            {
                foreach (int nubber in numbersToAdd)
                {
                    Numbers.Add(nubber);
                }
            }
        }

        public double GetMedian()
        {
            Numbers.Sort();
            double median;
            if (Numbers.Count % 2 == 0)
            {
                return (Numbers[Numbers.Count / 2] + Numbers[Numbers.Count / 2 - 1]) / 2.0;
            }

            return Numbers[(int)Math.Ceiling(Numbers.Count / 2.0)];
        }
    }
}