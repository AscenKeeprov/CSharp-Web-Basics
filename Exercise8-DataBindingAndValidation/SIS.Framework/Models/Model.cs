namespace SIS.Framework.Models
{
    public class Model
    {
	private bool? isValid;

	public bool? IsValid
	{
	    get => isValid;
	    set => isValid = isValid ?? value;
	}
    }
}
