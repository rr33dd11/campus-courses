using campus.AdditionalServices.TokenHelpers;
using campus.DBContext;
using campus.DBContext.Models;
using campus.DBContext.Models.DTO.AccountDTO;
using campus.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace campus.Controllers;


[ApiController]
[Route("/")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly TokenInteraction _tokenInteraction;

    public AccountController(IAccountService accountService, TokenInteraction tokenInteraction)
    {
        _accountService = accountService;
        _tokenInteraction = tokenInteraction;
    }
    
    [HttpPost("login")]
    [ProducesResponseType(typeof(TokenResponse), 200)]
    [ProducesResponseType(typeof(Error), 400)]
    [ProducesResponseType(typeof(Error), 500)]
    public async Task<TokenResponse> Login([FromBody] LoginRequest loginRequest)
    {
        return await _accountService.Login(loginRequest);
    }
    
    [HttpPost("registration")]
    [ProducesResponseType(typeof(TokenResponse), 200)]
    [ProducesResponseType(typeof(Error), 400)]
    [ProducesResponseType(typeof(Error), 500)]
    public async Task<TokenResponse> Registration([FromBody] RegistrationRequest registrationRequest)
    {
        return await _accountService.Registration(registrationRequest);
    }
    
    [Authorize(Policy = "BlackToken")]
    [HttpGet("profile")]
    [ProducesResponseType(typeof(GetProfileResponse), 200)]
    [ProducesResponseType(typeof(Error), 401)]
    [ProducesResponseType(typeof(Error), 500)]
    public async Task<GetProfileResponse> Profile()
    {
        var token = _tokenInteraction.GetTokenFromHeader();
        return await _accountService.GetProfile(token);
    }

    [Authorize(Policy = "BlackToken")]
    [HttpPut("profile")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(Error), 400)]
    [ProducesResponseType(typeof(Error), 401)]
    [ProducesResponseType(typeof(Error), 500)]
    public async Task<IActionResult> EditProfile([FromBody] EditProfileRequest editProfileRequest)
    {
        var token = _tokenInteraction.GetTokenFromHeader();
        await _accountService.EditProfile(token, editProfileRequest);
        return Ok();
    }
    
    [Authorize(Policy = "BlackToken")]
    [HttpPost("logout")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(Error), 401)]
    [ProducesResponseType(typeof(Error), 500)]
    public async Task<IActionResult> Logout()
    {
        var token = _tokenInteraction.GetTokenFromHeader();
        await _accountService.Logout(token);
        return Ok();
    }
}
