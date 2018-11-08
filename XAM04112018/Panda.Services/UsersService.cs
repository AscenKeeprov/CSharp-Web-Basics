using System.Collections.Generic;
using System.Linq;
using Panda.Data;
using Panda.Models;
using Panda.Models.Enumerations;
using Panda.Services.Contracts;

namespace Panda.Services
{
    public class UsersService : IUsersService
    {
	private readonly PandaDbContext context;

	public UsersService(PandaDbContext context)
	{
	    this.context = context;
	}

	public void AddUser(string username, string password, string email)
	{
	    Role role = Role.User;
	    if (!context.Users.Any()) role = Role.Admin;
	    User user = new User()
	    {
		Username = username,
		Password = password,
		Email = email,
		Role = role
	    };
	    context.Users.Add(user);
	    context.SaveChanges();
	}

	public bool Exists(string username)
	{
	    return context.Users.Any(u => u.Username == username);
	}

	public IEnumerable<User> GetAllUsers()
	{
	    return context.Users.AsEnumerable();
	}

	public User GetUserByUsername(string username)
	{
	    return context.Users.SingleOrDefault(u => u.Username == username);
	}
    }
}
