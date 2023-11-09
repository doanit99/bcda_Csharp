namespace ApiWebBanHang.Models
{
	public class Category
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Slug { get; set; }
		public int Parent_Id { get; set; }
		public int? Sort_Order { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.Now;
		public int? Status { get; set; }
	}
}
