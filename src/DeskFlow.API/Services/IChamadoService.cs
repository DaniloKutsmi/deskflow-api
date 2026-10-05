using DeskFlow.API.Models.Entities;

namespace DeskFlow.API.Services
{
    public interface IChamadoService
    {
        Task<Chamado> AbrirChamadoAsync(Chamado chamado);
        Task<Chamado?> IniciarAtendimentoAsync(int id);
        Task<Chamado?> EncerrarChamadoAsync(int id, string solucao);
        Task<Interacao?> AdicionarInteracaoAsync(int chamadoId, Interacao interacao);
    }
}