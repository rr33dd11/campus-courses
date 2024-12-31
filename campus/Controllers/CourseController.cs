using campus.AdditionalServices.TokenHelpers;
using campus.DBContext.DTO.CourseDTO;
using campus.DBContext.Models;
using campus.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace campus.Controllers;

[ApiController]
[Route("courses")]
public class CourseController: ControllerBase
{
    private readonly ICourseService _courseService;
    private readonly TokenInteraction _tokenInteraction;

    public CourseController(ICourseService courseService, TokenInteraction tokenInteraction)
    {
        _courseService = courseService;
        _tokenInteraction = tokenInteraction;
    }

    [Authorize(Policy = "BlackToken")]
    [HttpGet]
    [Route("{id}/details")]
    [ProducesResponseType(typeof(CourseDetailsResponse), 200)]
    [ProducesResponseType(typeof(Error), 401)]
    [ProducesResponseType(typeof(Error), 404)]
    [ProducesResponseType(typeof(Error), 500)]
    public async Task<CourseDetailsResponse> GetCourseDetails(Guid id)
    {
        var token = _tokenInteraction.GetTokenFromHeader();
        return await _courseService.GetCourseDetails(token, id);
    }
    
    [Authorize(Policy = "BlackToken")]
    [HttpPost]
    [Route("{id}/sign-up")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(Error), 400)]
    [ProducesResponseType(typeof(Error), 401)]
    [ProducesResponseType(typeof(Error), 500)]
    public async Task<IActionResult> SignUpOnCourse(Guid id)
    {
        var token = _tokenInteraction.GetTokenFromHeader();
        await _courseService.SignUpOnCourse(token, id);
        return Ok();
    }
    
    [Authorize(Policy = "BlackToken")]
    [HttpPut]
    [Route("{id}/status")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(Error), 400)]
    [ProducesResponseType(typeof(Error), 401)]
    [ProducesResponseType(typeof(Error), 403)]
    [ProducesResponseType(typeof(Error), 404)]
    [ProducesResponseType(typeof(Error), 500)]
    public async Task<IActionResult> EditCourseStatus([FromBody]EditCourseStatusRequest editCourseStatus, Guid id)
    {
        var token = _tokenInteraction.GetTokenFromHeader();
        await _courseService.EditCourseStatus(editCourseStatus, token, id);
        return Ok();
    }
    
    [Authorize(Policy = "BlackToken")]
    [HttpPut]
    [Route("{id}/student-status/{studentId}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(Error), 400)]
    [ProducesResponseType(typeof(Error), 401)]
    [ProducesResponseType(typeof(Error), 403)]
    [ProducesResponseType(typeof(Error), 404)]
    [ProducesResponseType(typeof(Error), 500)]
    public async Task<IActionResult> EditStudentStatus([FromBody]EditStudentStatusRequest editStudentStatus, Guid id, Guid studentId)
    {
        var token = _tokenInteraction.GetTokenFromHeader();
        await _courseService.EditStudentStatus(editStudentStatus, token, id, studentId);
        return Ok();
    }
    
    [Authorize(Policy = "BlackToken")]
    [HttpPost]
    [Route("{id}/notifications")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(Error), 400)]
    [ProducesResponseType(typeof(Error), 401)]
    [ProducesResponseType(typeof(Error), 403)]
    [ProducesResponseType(typeof(Error), 404)]
    [ProducesResponseType(typeof(Error), 500)]
    public async Task<IActionResult> CreateNotification([FromBody]CreateCourseNotificationRequest notification, Guid id)
    {
        var token = _tokenInteraction.GetTokenFromHeader();
        await _courseService.CreateNotification(notification, token, id);
        return Ok();
    }
    
    [Authorize(Policy = "BlackToken")]
    [HttpPut]
    [Route("{id}/marks/{studentId}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(Error), 400)]
    [ProducesResponseType(typeof(Error), 401)]
    [ProducesResponseType(typeof(Error), 403)]
    [ProducesResponseType(typeof(Error), 404)]
    [ProducesResponseType(typeof(Error), 500)]
    public async Task<IActionResult> EditStudentMarks([FromBody]EditStudentMarkRequest editMark, Guid id, Guid studentId)
    {
        var token = _tokenInteraction.GetTokenFromHeader();
        await _courseService.EditStudentMarks(editMark, token, id, studentId);
        return Ok();
    }

    [Authorize(Policy = "BlackToken")]
    [HttpPost]
    [Route("/group/{groupId}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(Error), 400)]
    [ProducesResponseType(typeof(Error), 401)]
    [ProducesResponseType(typeof(Error), 403)]
    [ProducesResponseType(typeof(Error), 404)]
    [ProducesResponseType(typeof(Error), 500)]
    public async Task<IActionResult> CreateCourse([FromBody]CreateCourseRequest createCourseRequest, Guid groupId)
    {
        var token = _tokenInteraction.GetTokenFromHeader();
        await _courseService.CreateCourse(createCourseRequest, token, groupId);
        return Ok();
    }
    
    [Authorize(Policy = "BlackToken")]
    [HttpPut]
    [Route("{id}/requirements-and-annotations")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(Error), 400)]
    [ProducesResponseType(typeof(Error), 401)]
    [ProducesResponseType(typeof(Error), 403)]
    [ProducesResponseType(typeof(Error), 404)]
    [ProducesResponseType(typeof(Error), 500)]
    
    public async Task<IActionResult> EditRequirementsAndAnnotations(EditAnnotationsAndRequirementsRequest editAnnotationsAndRequirementsRequest , Guid id)
    {
        var token = _tokenInteraction.GetTokenFromHeader();
        await _courseService.EditAnnotationsAndRequirements(editAnnotationsAndRequirementsRequest, token, id);
        return Ok();
    }

    [Authorize(Policy = "BlackToken")]
    [HttpPut]
    [Route("{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(Error), 400)]
    [ProducesResponseType(typeof(Error), 401)]
    [ProducesResponseType(typeof(Error), 403)]
    [ProducesResponseType(typeof(Error), 404)]
    [ProducesResponseType(typeof(Error), 500)]
    public async Task<IActionResult> EditCourse([FromBody] EditCourseRequest editCourseRequest, Guid id)
    {
        var token = _tokenInteraction.GetTokenFromHeader();
        await _courseService.EditCourse(editCourseRequest, token, id);
        return Ok();
    }
    
    [Authorize(Policy = "BlackToken")]
    [HttpDelete]
    [Route("{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(Error), 400)]
    [ProducesResponseType(typeof(Error), 401)]
    [ProducesResponseType(typeof(Error), 403)]
    [ProducesResponseType(typeof(Error), 404)]
    [ProducesResponseType(typeof(Error), 500)]
    public async Task<IActionResult> DeleteCourse(Guid id)
    {
        var token = _tokenInteraction.GetTokenFromHeader();
        await _courseService.DeleteCourse(token, id);
        return Ok();
    }
    
    [Authorize(Policy = "BlackToken")]
    [HttpPost]
    [Route("{id}/teachers")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(Error), 400)]
    [ProducesResponseType(typeof(Error), 401)]
    [ProducesResponseType(typeof(Error), 403)]
    [ProducesResponseType(typeof(Error), 404)]
    [ProducesResponseType(typeof(Error), 500)]
    public async Task<IActionResult> AddTeacher(Guid id, CreateTeacherRequest teacherRequest)
    {
        var token = _tokenInteraction.GetTokenFromHeader();
        await _courseService.CreateCourseTeacher(teacherRequest, token, id);
        return Ok();
    }
    
    
    [Authorize(Policy = "BlackToken")]
    [HttpGet]
    [Route("my")]
    [ProducesResponseType(typeof(List<CourseResponse>),200)]
    [ProducesResponseType(typeof(Error), 401)]
    [ProducesResponseType(typeof(Error), 500)]
    public async Task<List<CourseResponse>> GetMyCourses()
    {
        var token = _tokenInteraction.GetTokenFromHeader();
        return await _courseService.GetMyCourses(token);
    }

    [Authorize(Policy = "BlackToken")]
    [HttpGet]
    [Route("teaching")]
    [ProducesResponseType(typeof(List<CourseResponse>),200)]
    [ProducesResponseType(typeof(Error), 401)]
    [ProducesResponseType(typeof(Error), 500)]
    public async Task<List<CourseResponse>> GetTeachingCourses()
    {
        var token = _tokenInteraction.GetTokenFromHeader();
        return await _courseService.GetTeachingCourses(token);
    }
}