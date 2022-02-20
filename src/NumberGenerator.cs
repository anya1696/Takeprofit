using System.Net.NetworkInformation;
namespace TakeprofitTechnologyTestTask.src
{
    public static class NumberGenerator
    {
        static int MinNumber = 1;
        static int MaxNumber = 2018;
        static int CurrentNumber = MinNumber;
        public static int Next()
        {
            return CurrentNumber++;
        }

        public static bool IsFinished()
        {
            return CurrentNumber < MaxNumber;
        }
    }
}