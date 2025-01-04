namespace ProjectASPNET.Models
{
	public class CustomerViewModel
	{
		public int CustomerId { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string CountryName { get; set; }
		public string Email { get; set; }
		public int orderCount { get; set; }

		public virtual ICollection<CustOrder> CustOrders { get; set; } = new List<CustOrder>();

		public virtual ICollection<CustomerAddress> CustomerAddresses { get; set; } = new List<CustomerAddress>();
	}
}
