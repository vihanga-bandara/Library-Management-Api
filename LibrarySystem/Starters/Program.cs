namespace Starters
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var testDataNum = 10;
            var testDataBookTitle = "Moby Dick";
            var testDataRecurrenceNumber = 3;
            var testDataStartRange = 1;
            var testDataEndRange = 100;
               

            var question1Response = IsNumberPow2(testDataNum);
            Console.WriteLine(question1Response);
            var question2Response = ReverseString(testDataBookTitle);
            Console.WriteLine(question2Response);
            var question3Response = GenerateBookTitle(testDataBookTitle.Replace(" ", ""), testDataRecurrenceNumber);
            Console.WriteLine(question3Response);
            var question4Response = GetOddNumbers(testDataStartRange, testDataEndRange);
            Console.WriteLine(string.Join(", ", question4Response));
        }

        public static bool IsNumberPow2(int n)
        {
            //return System.Numerics.BitOperations.IsPow2(n);
            return n > 0 && ((n & (n - 1)) == 0);
        }

        public static string ReverseString(string s)
        {
            var reversedStr = s.ToCharArray();
            Array.Reverse(reversedStr);
            return new string(reversedStr);
        }

        public static string GenerateBookTitle(string bookTitle, int repetitionCount)
        {
            var finalBookTitle = "";

            for(int i = 0; i < repetitionCount; i++)
            {
                finalBookTitle += bookTitle;
            }

            return finalBookTitle;
        }

        public static IEnumerable<int> GetOddNumbers(int startRange, int endRange)
        {
            var oddNumbersArr = new List<int>();
            for(int i = startRange; i < endRange; i++)
            {
                if(i % 2 == 1)
                {
                    yield return i;
                }

            }
        }
    }
}

