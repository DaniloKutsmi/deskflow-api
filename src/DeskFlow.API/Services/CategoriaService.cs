using DeskFlow.API.Models.Entities;
using DeskFlow.API.Repositories;

namespace DeskFlow.API.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _repository;

        public CategoriaService(ICategoriaRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Categoria>> ListarAsync()
        {
            return await _repository.ObterTodasAsync();
        }

        public async Task<Categoria?> ObterPorIdAsync(int id)
        {
            return await _repository.ObterPorIdAsync(id);
        }

        public async Task<Categoria> CriarAsync(Categoria categoria)
        {
            await _repository.AdicionarAsync(categoria);
            await _repository.SalvarAsync();
            return categoria;
        }

        public async Task<bool> AtualizarAsync(int id, Categoria categoria)
        {
            var categoriaExistente = await _repository.ObterPorIdAsync(id);

            if (categoriaExistente == null)
            {
                return false;
            }

            categoriaExistente.Nome = categoria.Nome;
            _repository.Atualizar(categoriaExistente);
            await _repository.SalvarAsync();
            return true;
        }

        public async Task<string?> RemoverAsync(int id)
        {
            var categoria = await _repository.ObterPorIdAsync(id);

            if (categoria == null)
            {
                return "Categoria nao encontrada.";
            }

            var possuiChamados = await _repository.ExisteChamadoComCategoriaAsync(id);

            if (possuiChamados)
            {
                return "Nao e possivel remover uma categoria que possui chamados vinculados.";
            }

            _repository.Remover(categoria);
            await _repository.SalvarAsync();
            return null;
        }
    }
}