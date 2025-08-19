namespace CustomDSATrainer.Application.Services
{
    public class UserOutputService
    {
        private string _userOutput = string.Empty;

        public string? UserOutput
        {
            get => _userOutput;
            set => _userOutput = value;
        }

    }
}
