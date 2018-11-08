using Chushka.Models;

namespace Chushka.Services.Contracts
{
    public interface IUsersService
    {
	bool Exists(string username);
	User GetUserByUsername(string username);
	void AddUser(string username, string password, string email, string fullName);
    }
}
