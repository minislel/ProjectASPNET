namespace ProjectASPNET.Models
{
    public class OrderDetailsViewModel
    {
        public int OrderId { get; set; }
        public DateTime? OrderDate { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public string ShippingMethod { get; set; }
        public int CustomerId { get; set; }
        public Address DestAddress { get; set; }
        public int Status { get; set; }
        public Dictionary<int?, List<string>> BookWithPriceAndTitle { get; set; }
        public string CountryName { get; set; }
    }
}
