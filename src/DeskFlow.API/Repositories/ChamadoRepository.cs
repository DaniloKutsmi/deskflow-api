using Microsoft.EntityFrameworkCore;
using DeskFlow.API.Data;
using DeskFlow.API.Models.Entities;

namespace DeskFlow.API.Repositories
{
    public class ChamadoRepository : IChamadoRepository
    {
        private readonly AppDbContext _context;

        public ChamadoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AdicionarAsync(Chamado chamado)
        {
            await _context.Chamados.AddAsync(chamado);
        }

        public async Task<Chamado?> ObterPorIdAsync(int id)
        {
            return await _context.Chamados.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Chamado?> ObterDetalhesAsync(int id)
        {
            return await _context.Chamados
                .Include(c => c.Categoria)
                .Include(c => c.Interacoes)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Chamado>> ListarAsync(string? status, string? prioridade, int? categoriaId)
        {
            return await _context.Chamados
                .Include(c => c.Categoria)
                .Include(c => c.Interacoes)
                .Where(c => (status == null || c.Status == status)
                         && (prioridade == null || c.Prioridade == prioridade)
                         && (categoriaId == null || c.CategoriaId == categoriaId))
                .ToListAsync();
        }

        public void Atualizar(Chamado chamado)
        {
            _context.Chamados.Update(chamado);
        }

        public async Task AdicionarInteracaoAsync(Interacao interacao)
        {
            await _context.Interacoes.AddAsync(interacao);
        }

        public async Task<bool> SalvarAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}