using System.Collections.Generic;
using Panda.Models;

namespace Panda.Services.Contracts
{
    public interface IUsersService
    {
	bool Exists(string username);
	IEnumerable<User> GetAllUsers();
	User GetUserByUsername(string username);
	void AddUser(string username, string password, string email);
    }
}
