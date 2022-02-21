namespace TakeprofitTechnologyTestTask.src
{
    public class NumbersStorage
    {
        List<int> Numbers = new List<int>();
        Object loсker = new();
        
        public void AddNumber(int number)
        {
            lock (loсker)
            {
                Numbers.Add(number);
            }
        }

        public double GetMedian()
        {
            Numbers.Sort();
            if (Numbers.Count % 2 == 0)
            {
                return (Numbers[Numbers.Count / 2] + Numbers[Numbers.Count / 2 - 1]) / 2.0;
            }

            return Numbers[(int)Math.Ceiling(Numbers.Count / 2.0)];
        }
    }
}