using DeskFlow.API.Models.Entities;

namespace DeskFlow.API.Repositories
{
    public interface IChamadoRepository
    {
        Task AdicionarAsync(Chamado chamado);
        Task<bool> SalvarAsync();
    }
}