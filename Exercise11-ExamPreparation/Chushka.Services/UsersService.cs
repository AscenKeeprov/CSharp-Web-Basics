using System.Linq;
using Chushka.Data;
using Chushka.Models;
using Chushka.Models.Enumerations;
using Chushka.Services.Contracts;

namespace Chushka.Services
{
    public class UsersService : IUsersService
    {
	private readonly ChushkaDbContext context;

	public UsersService(ChushkaDbContext context)
	{
	    this.context = context;
	}

	public void AddUser(string username, string password, string email, string fullName)
	{
	    UserRole role = UserRole.User;
	    if (!context.Users.Any()) role = UserRole.Admin;
	    User user = new User()
	    {
		Username = username,
		Password = password,
		Email = email,
		FullName = fullName,
		Role = role
	    };
	    context.Users.Add(user);
	    context.SaveChanges();
	}

	public bool Exists(string username)
	{
	    return context.Users.Any(u => u.Username == username);
	}

	public User GetUserByUsername(string username)
	{
	    return context.Users
		.SingleOrDefault(u => u.Username == username);
	}
    }
}
