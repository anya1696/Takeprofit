using System.Net.NetworkInformation;
namespace TakeprofitTechnologyTestTask.src
{
    public static class NumberGenerator
    {
        static int MinNumber = 1;
        static int MaxNumber = 2018;
        static int CurrentNumber = MinNumber;
        static Object loсker = new();
        public static int Next()
        {
            lock(loсker){
            return CurrentNumber++;
            }
        }

        public static bool IsFinished()
        {
            return CurrentNumber <= MaxNumber;
        }
    }
}