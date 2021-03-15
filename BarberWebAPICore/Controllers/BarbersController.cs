using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BarberWebAPICore.Models;
using System.Net.Http;
using System.IO;
using System.Net.Http.Headers;

namespace BarberWebAPICore.Controllers
{
    //[EnableCors(origins: "http://localhost:3000/", headers: "*", methods: "*")]
    //[EnableCors("MyPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class BarbersController : ControllerBase
    {
        private readonly BarberAdminDB _context;

        public BarbersController(BarberAdminDB context)
        {
            _context = context;
        }

        // GET: api/Barbers
        [HttpGet]
        public async Task<ActionResult<List<Barber>>> GetBarbers()
        {
            var bs = await _context.Barbers.ToListAsync();
            return bs;
        }        

        // GET: api/Barbers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Barber>> GetBarber(int id)
        {
            var barber = await _context.Barbers.FindAsync(id);

            if (barber == null)
            {
                return NotFound();
            }

            return barber;
        }
        
        [HttpGet("GetImage/{id}")]
        public async Task<IActionResult> GetImage(int id)
        {
            var barber = await _context.Barbers.FindAsync(id);
            // return File(barber.PhotoFile, barber.ImageMimeType);
            var stream = new MemoryStream(barber.PhotoFile);

            // Compose a response containing the image and return to the user.
            var result = new HttpResponseMessage();

            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType =
                    new MediaTypeHeaderValue(barber.ImageMimeType);

            return File(barber.PhotoFile,
                        barber.ImageMimeType);
        }

        // PUT: api/Barbers/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBarber(int id, Barber barber)
        {
            if (id != barber.BarberID)
            {
                return BadRequest();
            }

            _context.Entry(barber).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BarberExists(id))
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

        // POST: api/Barbers
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Barber>> PostBarber(Barber barber)
        {
            _context.Barbers.Add(barber);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBarber", new { id = barber.BarberID }, barber);
        }

        // DELETE: api/Barbers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Barber>> DeleteBarber(int id)
        {
            var barber = await _context.Barbers.FindAsync(id);
            if (barber == null)
            {
                return NotFound();
            }

            _context.Barbers.Remove(barber);
            await _context.SaveChangesAsync();

            return barber;
        }

        private bool BarberExists(int id)
        {
            return _context.Barbers.Any(e => e.BarberID == id);
        }
    }
}
