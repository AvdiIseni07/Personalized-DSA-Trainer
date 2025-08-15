using CustomDSATrainer.Domain.Enums;
using CustomDSATrainer.Shared;

namespace CustomDSATrainer.Application
{
    public class TestCaseRunner
    {
        public TestCaseVerdict RunTest(string _expectedOutput)
        {
            TestCaseVerdict verdict = TestCaseVerdict.Passed;

            string[] userOutput = SharedValues.UserOutput.Split(new[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            string[] expectedOutput = _expectedOutput.Split(new[] {' ', '\n', '\r'}, StringSplitOptions.RemoveEmptyEntries);

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
