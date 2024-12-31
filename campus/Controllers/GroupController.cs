using campus.AdditionalServices.TokenHelpers;
using campus.DBContext;
using campus.DBContext.DTO.CourseDTO;
using campus.DBContext.DTO.GroupDTO;
using campus.DBContext.Models;
using campus.DBContext.Models.DTO.AccountDTO;
using campus.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace campus.Controllers;

[ApiController]
[Route("groups")]
public class GroupController : ControllerBase
{
    private readonly IGroupService _groupService;
    private readonly TokenInteraction _tokenInteraction;

    public GroupController(IGroupService groupService, TokenInteraction tokenInteraction)
    {
        _groupService = groupService;
        _tokenInteraction = tokenInteraction;
    }

    [Authorize(Policy = "BlackToken")]
    [HttpPost]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(Error), 401)]
    [ProducesResponseType(typeof(Error), 403)]
    [ProducesResponseType(typeof(Error), 500)]
    public async Task<IActionResult> CreateGroup([FromBody]CreateGroupRequest createGroupRequest)
    {
        var token = _tokenInteraction.GetTokenFromHeader();
        await _groupService.CreateGroup(createGroupRequest, token);
        return Ok();
    }
    
    [Authorize(Policy = "BlackToken")]
    [HttpPut("{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(Error), 401)]
    [ProducesResponseType(typeof(Error), 403)]
    [ProducesResponseType(typeof(Error), 404)]
    [ProducesResponseType(typeof(Error), 500)]
    public async Task<IActionResult> EditGroup([FromBody]EditGroupRequest editGroupRequest, Guid id)
    {
        var token = _tokenInteraction.GetTokenFromHeader();
        await _groupService.EditGroup(editGroupRequest, token, id);
        return Ok();
    }
    
    [Authorize(Policy = "BlackToken")]
    [HttpDelete("{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(Error), 401)]
    [ProducesResponseType(typeof(Error), 403)]
    [ProducesResponseType(typeof(Error), 404)]
    [ProducesResponseType(typeof(Error), 500)]
    public async Task<IActionResult> DeleteGroup(Guid id)
    {
        var token = _tokenInteraction.GetTokenFromHeader();
        await _groupService.DeleteGroup(token, id);
        return Ok();
    }

    [Authorize(Policy = "BlackToken")]
    [HttpGet]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(Error), 401)]
    [ProducesResponseType(typeof(Error), 500)]
    public async Task<List<GetGroupResponse>> GetGroups()
    {
        var token = _tokenInteraction.GetTokenFromHeader();
        return await _groupService.GetGroups(token);
    }

    [Authorize(Policy = "BlackToken")]
    [HttpGet("{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(Error), 401)]
    [ProducesResponseType(typeof(Error), 403)]
    [ProducesResponseType(typeof(Error), 404)]
    [ProducesResponseType(typeof(Error), 500)]
    public async Task<List<CourseResponse>> GetGroupCourses(Guid id)
    {
        var token = _tokenInteraction.GetTokenFromHeader();
        return await _groupService.GetCampusesByGroupId(token, id);
    }
}