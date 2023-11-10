using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiWebBanHang.Data;
using ApiWebBanHang.Models;
using Microsoft.AspNetCore.Cors;

namespace ApiWebBanHang.Controllers
{
	[EnableCors("AllowReactApp")]
	[Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly DataContext _context;
	

		
		public ProductsController(DataContext context)
        {
            _context = context;
           
        }

		// GET: api/Products
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
		{
			if (_context.Products == null)
			{
				return NotFound();
			}
			return await _context.Products.ToListAsync();
		}
		[HttpGet("{page}/{pageSize}")]
		public async Task<ActionResult<IEnumerable<Product>>> GetProductsPage(int page, int pageSize)
		{
			var totalProducts = await _context.Products.CountAsync();
			var totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);

			if (page < 1 || page > totalPages)
			{
				return BadRequest("Invalid page number");
			}

			var products = await _context.Products
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();

			if (products == null || products.Count == 0)
			{
				return NotFound();
			}

			return Ok(new
			{
				Page = page,
				PageSize = pageSize,
				TotalPages = totalPages,
				Data = products
			});
		}




		// GET: api/Products/5
		[HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
          if (_context.Products == null)
          {
              return NotFound();
          }
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

		// PUT: api/Products/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		//[HttpPut("{id}")]
		//public async Task<IActionResult> PutProduct(int id, Product product)
		//{
		//	if (id != product.Id)
		//	{
		//		return BadRequest();
		//	}

		//	_context.Entry(product).State = EntityState.Modified;

		//	try
		//	{
		//		await _context.SaveChangesAsync();
		//	}
		//	catch (DbUpdateConcurrencyException)
		//	{
		//		if (!ProductExists(id))
		//		{
		//			return NotFound();
		//		}
		//		else
		//		{
		//			throw;
		//		}
		//	}

		//	return NoContent();
		//}

		// PUT: api/Products/5
		[HttpPut("{id}")]
		public async Task<ActionResult> PutProduct(int id, [FromForm] Product product, IFormFile image)
		{
			//if (id != product.Id)
			//{
			//	return BadRequest();
			//}

			var existingProduct = await _context.Products.FindAsync(id);

			if (existingProduct == null)
			{
				return NotFound("Không tìm thấy sản phẩm có ID đã cho.");
			}

			if (image != null)
			{
				// Tạo tên tệp duy nhất cho hình ảnh
				string uniqueFileName = image.FileName;

				// Đường dẫn tới thư mục lưu trữ hình ảnh (dựa trên đường dẫn gốc của ứng dụng)
				string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

				// Đường dẫn tới tệp hình ảnh cụ thể
				string absolutePath = Path.Combine(uploadPath, uniqueFileName);

				// Lưu trữ hình ảnh vào thư mục lưu trữ
				using (var stream = new FileStream(absolutePath, FileMode.Create))
				{
					image.CopyTo(stream);
				}

				// Cập nhật trường "Hình" của sản phẩm với đường dẫn đến hình ảnh
				existingProduct.Image = uniqueFileName;
			}

			// Cập nhật thông tin sản phẩm
			existingProduct.Category_Id = product.Category_Id;
			existingProduct.Brand_Id = product.Brand_Id;
			existingProduct.Name = product.Name;
			existingProduct.Slug = product.Slug;
			existingProduct.Price = product.Price;
			existingProduct.Price_Sale = product.Price_Sale;
			existingProduct.Qty = product.Qty;
			existingProduct.Detail = product.Detail;
			existingProduct.Status = product.Status;

			try
			{
				_context.Update(existingProduct);
				await _context.SaveChangesAsync(); // Lưu thay đổi vào cơ sở dữ liệu
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!ProductExists(id))
				{
					return NotFound("Không tìm thấy sản phẩm có ID đã cho.");
				}
				else
				{
					throw;
				}
			}

			return Ok(existingProduct);
		}



		// POST: api/Products
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		//[HttpPost]
		//public async Task<ActionResult<Product>> PostProduct(Product product)
		//{
		//  if (_context.Products == null)
		//  {
		//      return Problem("Entity set 'DataContext.Products'  is null.");
		//  }
		//    _context.Products.Add(product);
		//    await _context.SaveChangesAsync();

		//    return CreatedAtAction("GetProduct", new { id = product.Id }, product);
		//}

		// POST: api/Products
		[HttpPost]
		public async Task<ActionResult<Product>> PostProduct([FromForm] Product product, IFormFile image)
		{
			if (image != null)
			{
                // Tạo tên tệp duy nhất cho hình ảnh
                string uniqueFileName = /*Guid.NewGuid().ToString() + "_" + */image.FileName;

                // Đường dẫn tới thư mục lưu trữ hình ảnh (dựa trên đường dẫn gốc của ứng dụng)
                string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

				// Đường dẫn tới tệp hình ảnh cụ thể
				string absolutePath = Path.Combine(uploadPath, uniqueFileName);

				// Lưu trữ hình ảnh vào thư mục lưu trữ
				using (var stream = new FileStream(absolutePath, FileMode.Create))
				{
					image.CopyTo(stream);
				}

				// Cập nhật trường "Hình" của sản phẩm với đường dẫn đến hình ảnh
				product.Image = /*"wwwroot/images/" + */uniqueFileName;

				// Thêm sản phẩm vào cơ sở dữ liệu
				_context.Products.Add(product);
				await _context.SaveChangesAsync(); // Lưu thay đổi vào cơ sở dữ liệu

				return Ok(product);
			}
			else
			{
				return BadRequest("Hình ảnh không hợp lệ");
			}
		}


		// DELETE: api/Products/5
		[HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (_context.Products == null)
            {
                return NotFound();
            }
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

		// GET: api/Products/5
		[HttpGet("{name?}")]
		public async Task<ActionResult<IEnumerable<Product>>> GetProductsSearch(string? name)
		{
			if (name == null)
			{
				return await _context.Products.ToListAsync();
			}
			if (_context.Products == null)
			{
				return NotFound();
			}
			var product = await _context.Products.Where(p => p.Name.Contains(name)).ToListAsync();

			if (product == null || product.Count == 0)
			{
				return NotFound();
			}

			return product;

		}
		

		private bool ProductExists(int id)
        {
            return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        }

       
    }
}
