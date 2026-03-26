using BackEnd.Modelos.SDR.DTO.LoginPortal;
using BackEnd.Servicos.SDR.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/portal")]
public class PortalController : ControllerBase
{
    private readonly LoginPortalService _loginPortalService;

    public PortalController(LoginPortalService loginPortalService)
    {
        _loginPortalService = loginPortalService;
    }

    [HttpGet]
    public async Task<IActionResult> GetLoginPortal()
    {
        var sdrUsers = await _loginPortalService.RetriveLeadsUsersAsync();
        return Ok(sdrUsers);
    }

    [HttpPut("{loginPortalId}")]
    public async Task<IActionResult> PutLoginPortalGroupAsync(int loginPortalId)
    {
        await _loginPortalService.ModifyLoginPortalGroupAsync(loginPortalId);
        return Ok();
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetSdrUsersLoginportal()
    {
        var sdrUsers = await _loginPortalService.RetriveSdrUsersAsync();
        return Ok(sdrUsers);
    }

    [HttpGet("vendedores")]
    public async Task<IActionResult> GetSellersAsync()
    {
        try
        {
            var sellers = await _loginPortalService.RetrieveSellersAsync();
            return Ok(sellers);
        }
        catch (HttpRequestException ex)
        {
            throw new HttpRequestException($"{ex.Message} Consulte o suporte.");
        }
    } // Completo
}