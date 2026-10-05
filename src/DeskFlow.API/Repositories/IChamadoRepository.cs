using DeskFlow.API.Models.Entities;

namespace DeskFlow.API.Repositories
{
    public interface IChamadoRepository
    {
        Task AdicionarAsync(Chamado chamado);
        Task<Chamado?> ObterPorIdAsync(int id);
        void Atualizar(Chamado chamado);
        Task AdicionarInteracaoAsync(Interacao interacao);
        Task<bool> SalvarAsync();
    }
}