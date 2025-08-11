using CustomDSATrainer.Domain;
using CustomDSATrainer.Domain.Enums;

namespace CustomDSATrainer.Application
{
    public class TestCaseRunner
    {
        private string userOutputFile = "AIService/UserOutput.txt";
        public TestCaseVerdict RunTest(string expectedOutputFile)
        {
            TestCaseVerdict verdict = TestCaseVerdict.Passed;

            string[] userOutput = File.ReadAllText(userOutputFile).Split(' ');
            string[] expectedOutput = File.ReadAllText(expectedOutputFile).Split(' ');

            if (userOutput.Length < expectedOutput.Length)
            {
                verdict = TestCaseVerdict.TooFewArguments;
                return verdict;
            }

            if (userOutput.Length > expectedOutput.Length)
            {
                verdict = TestCaseVerdict.TooManyArguments;
                return verdict;
            }

            for (int i = 0; i < userOutput.Length; i ++)
            {
                if (userOutput[i] != expectedOutput[i])
                {
                    verdict = TestCaseVerdict.IncorrectAnswer;
                    break;
                }
            }

            return verdict;
        }
    }
}
