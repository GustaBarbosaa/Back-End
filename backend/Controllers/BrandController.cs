using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infraestrutura.Repositories.Data;
using Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly EficazContext _context;

        public BrandController(EficazContext context)
        {
            _context = context;
        }

        // GET: api/Brand - Lista todas as marcas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Marca>>> GetBrands()
        {
            return await _context.Marcas.ToListAsync();
        }

        // GET: api/Brand/5 - Retorna uma marca específica pelo ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Marca>> GetBrand(int id)
        {
            var brand = await _context.Marcas.FindAsync(id);

            if (brand == null)
            {
                return NotFound();
            }

            return brand;
        }

        // POST: api/Brand - Adiciona uma nova marca
        [HttpPost]
        public async Task<ActionResult<Marca>> CreateBrand(Marca brand)
        {
            _context.Marcas.Add(brand);
            await _context.SaveChangesAsync();

            // Retorna a marca recém-criada com o ID gerado
            return CreatedAtAction(nameof(GetBrand), new { id = brand.Id }, brand);
        }

        // PUT: api/Brand/5 - Atualiza uma marca existente
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBrand(int id, Marca brand)
        {
            if (id != brand.Id)
            {
                return BadRequest();
            }

            _context.Entry(brand).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BrandExists(id))
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

        // DELETE: api/Brand/5 - Exclui uma marca pelo ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBrand(int id)
        {
            var brand = await _context.Marcas.FindAsync(id);
            if (brand == null)
            {
                return NotFound();
            }

            _context.Marcas.Remove(brand);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Verifica se uma marca existe pelo ID
        private bool BrandExists(int id)
        {
            return _context.Marcas.Any(e => e.Id == id);
        }
    }
}
