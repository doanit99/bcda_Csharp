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
    public class SlidersController : ControllerBase
    {
        private readonly DataContext _context;

        public SlidersController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Sliders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Slider>>> GetSliders()
        {
          if (_context.Sliders == null)
          {
              return NotFound();
          }
            return await _context.Sliders.ToListAsync();
        }

        // GET: api/Sliders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Slider>> GetSlider(int id)
        {
          if (_context.Sliders == null)
          {
              return NotFound();
          }
            var slider = await _context.Sliders.FindAsync(id);

            if (slider == null)
            {
                return NotFound();
            }

            return slider;
        }

        // PUT: api/Sliders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        //public async Task<IActionResult> PutSlider(int id, Slider slider)
        //{
        //    if (id != slider.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(slider).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!SliderExists(id))
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
		public async Task<ActionResult> PutSlider(int id, [FromForm] Slider slider, IFormFile image)
		{
			//if (id != slider.Id)
			//{
			//	return BadRequest();
			//}

			var existingSlider = await _context.Sliders.FindAsync(id);

			if (existingSlider == null)
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
				existingSlider.Image = uniqueFileName;
			}

			// Cập nhật thông tin sản phẩm
			
			existingSlider.Name = slider.Name;
			existingSlider.Link = slider.Link;
			existingSlider.Sort_order = slider.Sort_order;
			existingSlider.Status = slider.Status;

			try
			{
				_context.Update(existingSlider);
				await _context.SaveChangesAsync(); // Lưu thay đổi vào cơ sở dữ liệu
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!SliderExists(id))
				{
					return NotFound("Không tìm thấy ID đã cho.");
				}
				else
				{
					throw;
				}
			}

			return Ok(existingSlider);
		}

		// POST: api/Sliders
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

		//public async Task<ActionResult<Slider>> PostSlider(Slider slider)
		//{
		//  if (_context.Sliders == null)
		//  {
		//      return Problem("Entity set 'DataContext.Sliders'  is null.");
		//  }
		//    _context.Sliders.Add(slider);
		//    await _context.SaveChangesAsync();

		//    return CreatedAtAction("GetSlider", new { id = slider.Id }, slider);
		//}
		[HttpPost]
		public async Task<ActionResult<Slider>> PostSlider([FromForm] Slider slider, IFormFile image)
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
				slider.Image = /*"wwwroot/images/" + */uniqueFileName;

				// Thêm sản phẩm vào cơ sở dữ liệu
				_context.Sliders.Add(slider);
				await _context.SaveChangesAsync(); // Lưu thay đổi vào cơ sở dữ liệu

				return Ok(slider);
			}
			else
			{
				return BadRequest("Hình ảnh không hợp lệ");
			}
		}

		// DELETE: api/Sliders/5
		[HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSlider(int id)
        {
            if (_context.Sliders == null)
            {
                return NotFound();
            }
            var slider = await _context.Sliders.FindAsync(id);
            if (slider == null)
            {
                return NotFound();
            }

            _context.Sliders.Remove(slider);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SliderExists(int id)
        {
            return (_context.Sliders?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
