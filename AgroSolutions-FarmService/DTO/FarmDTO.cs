namespace AgroSolutions_FarmService.DTO;

public record PropriedadeCreateDto(string Nome, string Localizacao);
public record TalhaoCreateDto(string Descricao, string Cultura, int PropriedadeId);
public record PropriedadeResponseDto(int Id, string Nome, string Localizacao, List<TalhaoResponseDto> Talhoes);
public record TalhaoResponseDto(int Id, string Descricao, string Cultura);