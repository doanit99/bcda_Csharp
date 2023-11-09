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
using System.Drawing.Drawing2D;

namespace ApiWebBanHang.Controllers
{
	[EnableCors("AllowReactApp")]
	[Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly DataContext _context;

        public CategoriesController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
          if (_context.Categories == null)
          {
              return NotFound();
          }
            return await _context.Categories.ToListAsync();
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
          if (_context.Categories == null)
          {
              return NotFound();
          }
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return category;
        }

        // PUT: api/Categories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
		public async Task<IActionResult> PutCategory(int id, [FromForm] Category category)
		{

			var existingCategory = await _context.Categories.FindAsync(id);

			if (existingCategory == null)
			{
				return NotFound("Không tìm thấy ID đã cho.");
			}



			// Cập nhật thông tin sản phẩm
			existingCategory.Name = category.Name;
			existingCategory.Slug = category.Slug;
			existingCategory.Parent_Id = category.Parent_Id;
			existingCategory.Sort_Order = category.Sort_Order;
			existingCategory.Status = category.Status;

			try
			{
				_context.Update(existingCategory);
				await _context.SaveChangesAsync(); // Lưu thay đổi vào cơ sở dữ liệu
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!CategoryExists(id))
				{
					return NotFound("Không tìm thấy ID đã cho.");
				}
				else
				{
					throw;
				}
			}

			return Ok(existingCategory);
		}



		// POST: api/Categories
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
		public async Task<ActionResult<Category>> PostCategory([FromForm] Category category)
		{
			if (_context.Categories == null)
			{
				return Problem("Entity set 'DataContext.Categories' is null.");
			}

			

			_context.Categories.Add(category);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetCategory", new { id = category.Id }, category);
		}


		// DELETE: api/Categories/5
		[HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            if (_context.Categories == null)
            {
                return NotFound();
            }
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CategoryExists(int id)
        {
            return (_context.Categories?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
