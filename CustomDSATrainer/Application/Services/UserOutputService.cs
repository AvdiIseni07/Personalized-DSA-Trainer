using CustomDSATrainer.Domain.Interfaces.Services;

namespace CustomDSATrainer.Application.Services
{
    public class UserOutputService : IUserOutputService
    {
        private string _userOutput = string.Empty;

        public string? UserOutput
        {
            get => _userOutput;
            set => _userOutput = value;
        }

    }
}
