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
	[Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly DataContext _context;

        public BrandController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Brand
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Brand>>> GetBrands()
        {
          if (_context.Brands == null)
          {
              return NotFound();
          }
            return await _context.Brands.ToListAsync();
        }

        // GET: api/Brand/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Brand>> GetBrand(int id)
        {
          if (_context.Brands == null)
          {
              return NotFound();
          }
            var brand = await _context.Brands.FindAsync(id);

            if (brand == null)
            {
                return NotFound();
            }

            return brand;
        }

        // PUT: api/Brand/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        //public async Task<IActionResult> PutBrand(int id, Brand brand)
        //{
        //    //if (id != brand.Id)
        //    //{
        //    //    return BadRequest();
        //    //}

        //    _context.Entry(brand).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!BrandExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

		public async Task<ActionResult> PutBrand(int id, [FromForm] Brand brand, IFormFile image)
		{
			//if (id != brand.Id)
			//{
			//	return BadRequest();
			//}

			var existingBrand = await _context.Brands.FindAsync(id);

			if (existingBrand == null)
			{
				return NotFound("Không tìm thấy ID đã cho.");
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
				existingBrand.Image = uniqueFileName;
			}

			// Cập nhật thông tin sản phẩm
			existingBrand.Name = brand.Name;			
			existingBrand.Slug = brand.Slug;
			existingBrand.Sort_order = brand.Sort_order;
			existingBrand.Status = brand.Status;

			try
			{
				_context.Update(existingBrand);
				await _context.SaveChangesAsync(); // Lưu thay đổi vào cơ sở dữ liệu
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!BrandExists(id))
				{
					return NotFound("Không tìm thấy ID đã cho.");
				}
				else
				{
					throw;
				}
			}

			return Ok(existingBrand);
		}

		// POST: api/Brand
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
        //public async Task<ActionResult<Brand>> PostBrand(Brand brand)
        //{
        //  if (_context.Brands == null)
        //  {
        //      return Problem("Entity set 'DataContext.Brands'  is null.");
        //  }
        //    _context.Brands.Add(brand);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetBrand", new { id = brand.Id }, brand);
        //}
		public async Task<ActionResult<Brand>> PostBrand([FromForm] Brand brand, IFormFile image)
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
				brand.Image = /*"wwwroot/images/" + */uniqueFileName;

				// Thêm sản phẩm vào cơ sở dữ liệu
				_context.Brands.Add(brand);
				await _context.SaveChangesAsync(); // Lưu thay đổi vào cơ sở dữ liệu

				return Ok(brand);
			}
			else
			{
				return BadRequest("Hình ảnh không hợp lệ");
			}
		}

		// DELETE: api/Brand/5
		[HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBrand(int id)
        {
            if (_context.Brands == null)
            {
                return NotFound();
            }
            var brand = await _context.Brands.FindAsync(id);
            if (brand == null)
            {
                return NotFound();
            }

            _context.Brands.Remove(brand);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BrandExists(int id)
        {
            return (_context.Brands?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
