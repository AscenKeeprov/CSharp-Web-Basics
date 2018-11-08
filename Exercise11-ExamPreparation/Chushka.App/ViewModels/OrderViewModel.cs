namespace Chushka.App.ViewModels
{
    public class OrderViewModel
    {
	public int OrderId { get; set; }
	public string CustomerName { get; set; }
	public string ProductName { get; set; }
	public string OrderDate { get; set; }
	public int RowNumber => OrderId;
	public string RowColour => OrderId % 2 != 0 ? " chushka-bg-color text-white" : " bg-white";
    }
}
