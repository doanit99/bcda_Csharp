using ApiWebBanHang.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiWebBanHang.Data
{
	public class DataContext:DbContext
	{
		public DataContext(DbContextOptions<DataContext> options) : base(options) 
		{

		}

		public DbSet<Category> Categories { get; set; }
		public DbSet<Brand> Brands { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<Slider> Sliders { get; set; }
	}
}
