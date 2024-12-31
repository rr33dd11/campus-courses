using campus.AdditionalServices.TokenHelpers;
using campus.DBContext;
using campus.DBContext.DTO.UsersDTO;
using campus.DBContext.Models;
using campus.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace campus.Controllers;

[ApiController]
[Route("/")]
public class UsersController : ControllerBase
{
    
    private readonly IUsersService _usersService;
    private readonly TokenInteraction _tokenInteraction;

    public UsersController(IUsersService usersService, TokenInteraction tokenInteraction)
    {
        _usersService = usersService;
        _tokenInteraction = tokenInteraction;
    }
    
    [Authorize(Policy = "BlackToken")]
    [HttpGet("users")]
    [ProducesResponseType(typeof(List<UserResponse>), 200)]
    [ProducesResponseType(typeof(Error), 401)]
    [ProducesResponseType(typeof(Error), 403)]
    [ProducesResponseType(typeof(Error), 500)]
    public async Task<List<UserResponse>> GetUsers()
    {
        var token = _tokenInteraction.GetTokenFromHeader();
        return await _usersService.GetUsers(token);
    }

    [Authorize(Policy = "BlackToken")]
    [HttpGet("roles")]
    [ProducesResponseType(typeof(RolesResponse), 200)]
    [ProducesResponseType(typeof(Error), 401)]
    [ProducesResponseType(typeof(Error), 500)]
    public async Task<RolesResponse> GetRoles()
    {
        var token = _tokenInteraction.GetTokenFromHeader();
        return await _usersService.GetRoles(token);
    }
}