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

        public void Atualizar(Chamado chamado)
        {
            _context.Chamados.Update(chamado);
        }

        public async Task<bool> SalvarAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}