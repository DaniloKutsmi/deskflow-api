using DeskFlow.API.Models.Entities;

namespace DeskFlow.API.Services
{
    public interface ICategoriaService
    {
        Task<List<Categoria>> ListarAsync();
        Task<Categoria?> ObterPorIdAsync(int id);
        Task<Categoria> CriarAsync(Categoria categoria);
        Task<bool> AtualizarAsync(int id, Categoria categoria);
        Task<string?> RemoverAsync(int id);
    }
}