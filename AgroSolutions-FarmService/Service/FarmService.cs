using AgroSolutions_FarmService.DTO;
using AgroSolutions_FarmService.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.EntityFrameworkCore; 

namespace AgroSolutions_FarmService.Service
{
    public class FarmService : IFarmService
    {
        private readonly AgroSolutions.FarmService.Data.FarmDbContext _context;
        public FarmService(AgroSolutions.FarmService.Data.FarmDbContext context) => _context = context;

        public async Task<PropriedadeResponseDto> CriarPropriedadeAsync(PropriedadeCreateDto dto, string produtorId)
        {
            var propriedade = new Propriedade { Nome = dto.Nome, Localizacao = dto.Localizacao, ProdutorId = produtorId };
            _context.Propriedades.Add(propriedade);
            await _context.SaveChangesAsync();
            return new PropriedadeResponseDto(propriedade.Id, propriedade.Nome, propriedade.Localizacao, new());
        }

        public async Task<TalhaoResponseDto> AdicionarTalhaoAsync(TalhaoCreateDto dto, string produtorId)
        {
            // Valida se a propriedade pertence ao produtor logado
            var prop = await _context.Propriedades.FindAsync(dto.PropriedadeId);
            if (prop == null || prop.ProdutorId != produtorId) throw new UnauthorizedAccessException("Propriedade inválida.");

            var talhao = new Talhao { Descricao = dto.Descricao, Cultura = dto.Cultura, PropriedadeId = dto.PropriedadeId };
            _context.Talhoes.Add(talhao);
            await _context.SaveChangesAsync();
            return new TalhaoResponseDto(talhao.Id, talhao.Descricao, talhao.Cultura);
        }

        public async Task<IEnumerable<PropriedadeResponseDto>> ListarPropriedadesAsync(string produtorId)
        {
            return await _context.Propriedades
                .Where(p => p.ProdutorId == produtorId)
                .Select(p => new PropriedadeResponseDto(p.Id, p.Nome, p.Localizacao,
                    p.Talhoes.Select(t => new TalhaoResponseDto(t.Id, t.Descricao, t.Cultura)).ToList()))
                .ToListAsync();
        }
    }
}
