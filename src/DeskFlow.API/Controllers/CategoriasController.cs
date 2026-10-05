using Microsoft.AspNetCore.Mvc;
using DeskFlow.API.Models.Entities;
using DeskFlow.API.Services;

namespace DeskFlow.API.Controllers
{
    [ApiController]
    [Route("api/categorias")]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaService _service;

        public CategoriasController(ICategoriaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodas()
        {
            var categorias = await _service.ListarAsync();
            return Ok(categorias);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var categoria = await _service.ObterPorIdAsync(id);

            if (categoria == null)
            {
                return NotFound("Categoria nao encontrada.");
            }

            return Ok(categoria);
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] Categoria categoria)
        {
            var categoriaCriada = await _service.CriarAsync(categoria);
            return CreatedAtAction(nameof(ObterPorId), new { id = categoriaCriada.Id }, categoriaCriada);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] Categoria categoria)
        {
            var atualizado = await _service.AtualizarAsync(id, categoria);

            if (!atualizado)
            {
                return NotFound("Categoria nao encontrada.");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remover(int id)
        {
            var erro = await _service.RemoverAsync(id);

            if (erro != null)
            {
                return BadRequest(erro);
            }

            return NoContent();
        }
    }
}