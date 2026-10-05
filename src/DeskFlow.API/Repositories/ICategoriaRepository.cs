using DeskFlow.API.Models.Entities;

namespace DeskFlow.API.Repositories
{
    public interface ICategoriaRepository
    {
        Task<List<Categoria>> ObterTodasAsync();
        Task<Categoria?> ObterPorIdAsync(int id);
        Task AdicionarAsync(Categoria categoria);
        void Atualizar(Categoria categoria);
        void Remover(Categoria categoria);
        Task<bool> SalvarAsync();
        Task<bool> ExisteChamadoComCategoriaAsync(int categoriaId);
    }
}