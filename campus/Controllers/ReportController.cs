using campus.AdditionalServices.TokenHelpers;
using campus.DBContext.DTO.ReportDTO;
using campus.DBContext.Models;
using campus.DBContext.Query;
using campus.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace campus.Controllers;

[ApiController]
[Route("/report")]
public class ReportController : ControllerBase
{
    
    private readonly IReportService _reportService;
    private readonly TokenInteraction _tokenInteraction;

    public ReportController(IReportService reportService, TokenInteraction tokenInteraction)
    {
        _reportService = reportService;
        _tokenInteraction = tokenInteraction;
    }
    
    [Authorize(Policy = "BlackToken")]
    [HttpGet]
    [ProducesResponseType(typeof(List<ReportRecord>), 200)]
    [ProducesResponseType(typeof(Error), 401)]
    [ProducesResponseType(typeof(Error), 403)]
    [ProducesResponseType(typeof(Error), 500)]
    public async Task<List<ReportRecord>> GetReport([FromQuery] ReportQuery query)
    {
        var token = _tokenInteraction.GetTokenFromHeader();
        return await _reportService.GenerateReport(query, token);
    }
}