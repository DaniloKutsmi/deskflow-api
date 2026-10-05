using Microsoft.AspNetCore.Mvc;
using DeskFlow.API.Models;
using DeskFlow.API.Models.Entities;
using DeskFlow.API.Models.Exceptions;
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

        [HttpPost("{id}/iniciar")]
        public async Task<IActionResult> Iniciar(int id)
        {
            try
            {
                var chamado = await _service.IniciarAtendimentoAsync(id);

                if (chamado == null)
                {
                    return NotFound("Chamado nao encontrado.");
                }

                return Ok(chamado);
            }
            catch (RegraNegocioException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{id}/encerrar")]
        public async Task<IActionResult> Encerrar(int id, [FromBody] EncerrarChamadoRequest request)
        {
            try
            {
                var chamado = await _service.EncerrarChamadoAsync(id, request.Solucao);

                if (chamado == null)
                {
                    return NotFound("Chamado nao encontrado.");
                }

                return Ok(chamado);
            }
            catch (RegraNegocioException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{id}/interacoes")]
        public async Task<IActionResult> AdicionarInteracao(int id, [FromBody] Interacao interacao)
        {
            try
            {
                var interacaoCriada = await _service.AdicionarInteracaoAsync(id, interacao);

                if (interacaoCriada == null)
                {
                    return NotFound("Chamado nao encontrado.");
                }

                return StatusCode(201, interacaoCriada);
            }
            catch (RegraNegocioException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}