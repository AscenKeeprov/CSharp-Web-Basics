using System.Collections.Generic;
using Chushka.Models.Enumerations;

namespace Chushka.Models
{
    public class User
    {
	public User()
	{
	    Orders = new HashSet<Order>();
	}

	public int Id { get; set; }
	public string Username { get; set; }
	public string Password { get; set; }
	public string FullName { get; set; }
	public string Email { get; set; }
	public UserRole Role { get; set; }
	public bool IsDeleted { get; set; }
	public virtual IEnumerable<Order> Orders { get; set; }
    }
}
