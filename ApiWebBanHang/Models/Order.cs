namespace ApiWebBanHang.Models
{
	public class Order
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string? Note { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.Now;
		public int? Status { get; set; }



	}
}
