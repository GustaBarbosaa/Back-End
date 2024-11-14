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
    public class ProductController : ControllerBase
    {
        private readonly EficazContext _context;

        public ProductController(EficazContext context)
        {
            _context = context;
        }

        // GET: api/Product - Lista todos os produtos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> GetProducts()
        {
            return await _context.Produtos.Include(p => p.Marca).ToListAsync();
        }

        // GET: api/Product/5 - Retorna um produto específico pelo ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Produto>> GetProduct(int id)
        {
            var product = await _context.Produtos.Include(p => p.Marca).FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // POST: api/Product - Adiciona um novo produto com MarcaId
        [HttpPost]
        public async Task<ActionResult<Produto>> CreateProduct(Produto product)
        {
            // Verifica se o MarcaId fornecido existe
            var marca = await _context.Marcas.FindAsync(product.MarcaId);
            if (marca == null)
            {
                return BadRequest("MarcaId inválido. A marca especificada não foi encontrada.");
            }

            // Adiciona o produto ao contexto e salva
            _context.Produtos.Add(product);
            await _context.SaveChangesAsync();

            // Retorna o produto recém-criado com o ID gerado
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        // PUT: api/Product/5 - Atualiza um produto existente
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Produto product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            // Verifica se o MarcaId fornecido existe
            var marca = await _context.Marcas.FindAsync(product.MarcaId);
            if (marca == null)
            {
                return BadRequest("MarcaId inválido. A marca especificada não foi encontrada.");
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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

        // DELETE: api/Product/5 - Exclui um produto pelo ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Produtos.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Produtos.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Verifica se um produto existe pelo ID
        private bool ProductExists(int id)
        {
            return _context.Produtos.Any(e => e.Id == id);
        }
    }
}
