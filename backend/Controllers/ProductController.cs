using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infraestrutura.Repositories.Data;
using Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.DTOs;
using System.IO;

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
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
        {
            var products = await _context.Produtos
                .Select(p => new ProductDTO
                {
                    Id = p.Id,
                    Nome = p.Nome,
                    Preco = p.Preco,
                    MarcaId = p.MarcaId,
                    Imagem = p.Imagem
                })
                .ToListAsync();

            return Ok(products);  // Garante que estamos retornando uma resposta Ok com a lista de produtos
        }

        // GET: api/Product/5 - Retorna um produto específico pelo ID
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            var product = await _context.Produtos
                .Where(p => p.Id == id)
                .Select(p => new ProductDTO
                {
                    Id = p.Id,
                    Nome = p.Nome,
                    Preco = p.Preco,
                    MarcaId = p.MarcaId,
                    Imagem = p.Imagem
                })
                .FirstOrDefaultAsync();

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // POST: api/Product - Adiciona um novo produto com MarcaId e Imagem (IFormFile)
        [HttpPost]
        public async Task<ActionResult<ProductDTO>> CreateProduct([FromForm] ProductDTO productDto, IFormFile ImagemArquivo)
        {
            var marca = await _context.Marcas.FindAsync(productDto.MarcaId);
            if (marca == null)
            {
                return BadRequest("MarcaId inválido. A marca especificada não foi encontrada.");
            }

            // Salva a imagem no servidor, se fornecida
            string imagemCaminho = null;
            if (ImagemArquivo != null && ImagemArquivo.Length > 0)
            {
                var uploadsFolderPath = Path.Combine("wwwroot", "imagens_produtos");
                Directory.CreateDirectory(uploadsFolderPath);

                var filePath = Path.Combine(uploadsFolderPath, ImagemArquivo.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImagemArquivo.CopyToAsync(stream);
                }
                imagemCaminho = $"/imagens_produtos/{ImagemArquivo.FileName}"; // Caminho relativo para acesso
            }

            var product = new Produto
            {
                Nome = productDto.Nome,
                Preco = productDto.Preco,
                MarcaId = productDto.MarcaId,
                Imagem = imagemCaminho
            };

            _context.Produtos.Add(product);
            await _context.SaveChangesAsync();

            productDto.Id = product.Id;
            productDto.Imagem = imagemCaminho;
            return CreatedAtAction(nameof(GetProduct), new { id = productDto.Id }, productDto);
        }

        // PUT: api/Product/5 - Atualiza um produto existente com imagem (IFormFile)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] ProductDTO productDto, IFormFile imagem)
        {
            if (id != productDto.Id)
            {
                return BadRequest();
            }

            var marca = await _context.Marcas.FindAsync(productDto.MarcaId);
            if (marca == null)
            {
                return BadRequest("MarcaId inválido. A marca especificada não foi encontrada.");
            }

            var product = await _context.Produtos.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            // Atualiza a imagem no servidor, se fornecida
            if (imagem != null && imagem.Length > 0)
            {
                var uploadsFolderPath = Path.Combine("wwwroot", "imagens_produtos");
                Directory.CreateDirectory(uploadsFolderPath);

                var filePath = Path.Combine(uploadsFolderPath, imagem.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imagem.CopyToAsync(stream);
                }
                product.Imagem = $"/imagens_produtos/{imagem.FileName}"; // Caminho relativo para acesso
            }

            product.Nome = productDto.Nome;
            product.Preco = productDto.Preco;
            product.MarcaId = productDto.MarcaId;

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
