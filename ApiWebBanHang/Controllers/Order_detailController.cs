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
    public class Order_detailController : ControllerBase
    {
        private readonly DataContext _context;

        public Order_detailController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Order_detail
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order_detail>>> GetOrder_details()
        {
          if (_context.Order_details == null)
          {
              return NotFound();
          }
            return await _context.Order_details.ToListAsync();
        }

        // GET: api/Order_detail/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order_detail>> GetOrder_detail(int id)
        {
          if (_context.Order_details == null)
          {
              return NotFound();
          }
            var order_detail = await _context.Order_details.FindAsync(id);

            if (order_detail == null)
            {
                return NotFound();
            }

            return order_detail;
        }

        // PUT: api/Order_detail/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder_detail(int id, [FromForm] Order_detail order_detail)
        {
            if (id != order_detail.Id)
            {
                return BadRequest();
            }

            _context.Entry(order_detail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Order_detailExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Order_detail
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Order_detail>> PostOrder_detail([FromForm] Order_detail order_detail)
        {
          if (_context.Order_details == null)
          {
              return Problem("Entity set 'DataContext.Order_details'  is null.");
          }
            _context.Order_details.Add(order_detail);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrder_detail", new { id = order_detail.Id }, order_detail);
        }

        // DELETE: api/Order_detail/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder_detail(int id)
        {
            if (_context.Order_details == null)
            {
                return NotFound();
            }
            var order_detail = await _context.Order_details.FindAsync(id);
            if (order_detail == null)
            {
                return NotFound();
            }

            _context.Order_details.Remove(order_detail);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool Order_detailExists(int id)
        {
            return (_context.Order_details?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
