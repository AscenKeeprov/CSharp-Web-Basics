namespace IRunes.App.Services.Contracts
{
    public interface IEncryptionService
    {
	string HashPassword(string password);
    }
}
