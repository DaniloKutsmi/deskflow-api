using DeskFlow.API.Models.Entities;
using DeskFlow.API.Models.Exceptions;
using DeskFlow.API.Repositories;

namespace DeskFlow.API.Services
{
    public class ChamadoService : IChamadoService
    {
        private readonly IChamadoRepository _repository;

        public ChamadoService(IChamadoRepository repository)
        {
            _repository = repository;
        }

        public async Task<Chamado> AbrirChamadoAsync(Chamado chamado)
        {
            chamado.Status = "Aberto";
            chamado.DataAbertura = DateTime.Now;
            chamado.DataFechamento = null;
            chamado.Solucao = null;

            await _repository.AdicionarAsync(chamado);
            await _repository.SalvarAsync();

            return chamado;
        }

        public async Task<Chamado?> IniciarAtendimentoAsync(int id)
        {
            var chamado = await _repository.ObterPorIdAsync(id);

            if (chamado == null)
            {
                return null;
            }

            if (chamado.Status != "Aberto")
            {
                throw new RegraNegocioException("Somente chamados com status Aberto podem ser iniciados.");
            }

            chamado.Status = "EmAndamento";

            _repository.Atualizar(chamado);
            await _repository.SalvarAsync();

            return chamado;
        }

        public async Task<Chamado?> EncerrarChamadoAsync(int id, string solucao)
        {
            var chamado = await _repository.ObterPorIdAsync(id);

            if (chamado == null)
            {
                return null;
            }

            if (chamado.Status != "EmAndamento")
            {
                throw new RegraNegocioException("Somente chamados com status EmAndamento podem ser encerrados.");
            }

            chamado.Solucao = solucao;
            chamado.DataFechamento = DateTime.Now;
            chamado.Status = "Fechado";

            _repository.Atualizar(chamado);
            await _repository.SalvarAsync();

            return chamado;
        }

        public async Task<Interacao?> AdicionarInteracaoAsync(int chamadoId, Interacao interacao)
        {
            var chamado = await _repository.ObterPorIdAsync(chamadoId);

            if (chamado == null)
            {
                return null;
            }

            if (chamado.Status == "Fechado")
            {
                throw new RegraNegocioException("Nao e possivel adicionar interacao em um chamado Fechado.");
            }

            interacao.ChamadoId = chamadoId;
            interacao.DataRegistro = DateTime.Now;

            await _repository.AdicionarInteracaoAsync(interacao);
            await _repository.SalvarAsync();

            return interacao;
        }

        public async Task<Chamado?> ObterDetalhesAsync(int id)
        {
            return await _repository.ObterDetalhesAsync(id);
        }

        public async Task<List<Chamado>> ListarAsync(string? status, string? prioridade, int? categoriaId)
        {
            return await _repository.ListarAsync(status, prioridade, categoriaId);
        }
    }
}