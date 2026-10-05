using Microsoft.AspNetCore.Mvc;
using DeskFlow.API.Models.Entities;
using DeskFlow.API.Services;

namespace DeskFlow.API.Controllers
{
    [ApiController]
    [Route("api/chamados")]
    public class ChamadosController : ControllerBase
    {
        private readonly IChamadoService _service;

        public ChamadosController(IChamadoService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Abrir([FromBody] Chamado chamado)
        {
            var chamadoAberto = await _service.AbrirChamadoAsync(chamado);
            return StatusCode(201, chamadoAberto);
        }
    }
}