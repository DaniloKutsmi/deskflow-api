using DeskFlow.API.Models.Entities;

namespace DeskFlow.API.Services
{
    public interface IChamadoService
    {
        Task<Chamado> AbrirChamadoAsync(Chamado chamado);
    }
}