using DeskFlow.API.Models.Entities;
using DeskFlow.API.Repositories;

namespace DeskFlow.API.Services
{
    public class ChamadoService : IChamadoService
    {
        private readonly IChamadoRepository _repository;

        public ChamadoService(IChamadoRepository repository)
        {
            _repository = repository;
        }

        public async Task<Chamado> AbrirChamadoAsync(Chamado chamado)
        {
            chamado.Status = "Aberto";
            chamado.DataAbertura = DateTime.Now;
            chamado.DataFechamento = null;
            chamado.Solucao = null;

            await _repository.AdicionarAsync(chamado);
            await _repository.SalvarAsync();

            return chamado;
        }
    }
}