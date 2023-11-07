namespace ApiWebBanHang.Models
{
	public class Slider
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Link { get; set; }
		public string Image { get; set; }
		public int? Sort_order { get; set; }
		public DateTime CreatedAt { get; set; }

		public int? Status { get; set; }
	}
}
