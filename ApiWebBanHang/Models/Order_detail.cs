namespace ApiWebBanHang.Models
{
	public class Order_detail
	{
		public int Id { get; set; }
		public int Order_id { get; set; }
		public int Product_id {  get; set; }
		public int Qty { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.Now;
	}
}
