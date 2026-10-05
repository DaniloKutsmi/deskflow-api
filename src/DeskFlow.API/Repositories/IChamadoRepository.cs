using DeskFlow.API.Models.Entities;

namespace DeskFlow.API.Repositories
{
    public interface IChamadoRepository
    {
        Task AdicionarAsync(Chamado chamado);
        Task<Chamado?> ObterPorIdAsync(int id);
        Task<Chamado?> ObterDetalhesAsync(int id);
        Task<List<Chamado>> ListarAsync(string? status, string? prioridade, int? categoriaId);
        void Atualizar(Chamado chamado);
        Task AdicionarInteracaoAsync(Interacao interacao);
        Task<bool> SalvarAsync();
    }
}