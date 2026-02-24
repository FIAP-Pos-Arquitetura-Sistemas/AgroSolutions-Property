using AgroSolutions_FarmService.DTO;

namespace AgroSolutions_FarmService.Service
{
    public interface IFarmService
    {
        Task<PropriedadeResponseDto> CriarPropriedadeAsync(PropriedadeCreateDto dto, string produtorId);
        Task<TalhaoResponseDto> AdicionarTalhaoAsync(TalhaoCreateDto dto, string produtorId);
        Task<IEnumerable<PropriedadeResponseDto>> ListarPropriedadesAsync(string produtorId);
    }
}
