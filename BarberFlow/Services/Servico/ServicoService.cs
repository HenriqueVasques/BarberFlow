
using BarberFlow.API.Data.Repositories;
using BarberFlow.API.DTOs.Servico;
using BarberFlow.API.Interfaces;
using BarberFlow.API.Models;

namespace BarberFlow.API.Services
{
    public class ServicoService
    {
        private readonly IServicoRepository _servicoRepository;
        private readonly IEmpresaRepository _empresaRepository;

        public ServicoService(IServicoRepository servicoRepository, IEmpresaRepository empresaRepository) 
        {
            _servicoRepository = servicoRepository;
            _empresaRepository = empresaRepository;
        }

        public async Task<Servico> CriarServico(ServicoCreateDto dto)
        {
            var empresa = await _empresaRepository.ObterPorId(dto.EmpresaId);

            if(empresa == null)
            {
                throw new Exception("Empresa não encontrada.");
            }
            var servico = new Servico(dto.Nome, dto.DuracaoMinutos, dto.PrecoBase, dto.EmpresaId);
            await _servicoRepository.Adicionar(servico);
            return servico;
        }

        public async Task<IEnumerable<Servico>> ObterServicosPorEmpresa(long empresaId)
        {
            var empresa = await _empresaRepository.ObterPorId(empresaId);
            if (empresa == null)
            {
                throw new Exception("Empresa não encontrada.");
            }
            return await _servicoRepository.ObterPorEmpresa(empresaId);
        }
    }
}
