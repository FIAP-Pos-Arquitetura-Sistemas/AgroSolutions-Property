using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using AgroSolutions_FarmService.DTO;
using AgroSolutions_FarmService.Service;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Exige o Token do IdentityService
public class FarmController : ControllerBase
{
    private readonly IFarmService _farmService;
    public FarmController(IFarmService farmService) => _farmService = farmService;

    private string GetUserId() => User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;

    [HttpPost("propriedade")]
    public async Task<IActionResult> CriarPropriedade(PropriedadeCreateDto dto)
        => Ok(await _farmService.CriarPropriedadeAsync(dto, GetUserId()));

    [HttpPost("talhao")]
    public async Task<IActionResult> AdicionarTalhao(TalhaoCreateDto dto)
        => Ok(await _farmService.AdicionarTalhaoAsync(dto, GetUserId()));

    [HttpGet]
    public async Task<IActionResult> ListarMinhasFazendas()
        => Ok(await _farmService.ListarPropriedadesAsync(GetUserId()));
}