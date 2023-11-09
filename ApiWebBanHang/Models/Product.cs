using System.ComponentModel.DataAnnotations.Schema;

namespace ApiWebBanHang.Models
{
	public class Product
	{
		public int Id { get; set; }
		public int Category_Id { get; set; }
		public int Brand_Id { get; set; }
		public string Name { get; set; }
		public string Slug { get; set; }
		public double Price { get; set; }
		public double Price_Sale { get; set; }
		public int Qty { get; set; }
		public string? Image { get; set; }
		public string? Detail { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.Now;
		public int? Status { get; set; }

		
	}
}
