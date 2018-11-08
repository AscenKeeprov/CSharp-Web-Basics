using System.Collections.Generic;
using Panda.Models.Enumerations;

namespace Panda.Models
{
    public class User
    {
	public User()
	{
	    Packages = new HashSet<Package>();
	    Receipts = new HashSet<Receipt>();
	}

	public int Id { get; set; }
	public string Username { get; set; }
	public string Password { get; set; }
	public string Email { get; set; }
	public Role Role { get; set; }

	public virtual IEnumerable<Package> Packages { get; set; }
	public virtual IEnumerable<Receipt> Receipts { get; set; }
    }
}
