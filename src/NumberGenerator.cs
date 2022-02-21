namespace TakeprofitTechnologyTestTask.src
{
    public class NumberGenerator
    {
        int MinNumber;
        int MaxNumber;
        int CurrentNumber;
        Object loсker = new();
        public NumberGenerator(int minNumber = 1, int maxNumber = 2018)
        {
            MinNumber = minNumber;
            MaxNumber = maxNumber;
            CurrentNumber = MinNumber;
        }
        public int Next()
        {
            lock (loсker)
            {
                return CurrentNumber++;
            }
        }

        public bool HasNext()
        {
            return CurrentNumber <= MaxNumber;
        }
    }
}