namespace ApiWebBanHang.Models
{
	public class Brand
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Slug { get; set; }
		public string? Image { get; set; }
		public int? Sort_order { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.Now;

		public int? Status  { get; set; }

	}
}
