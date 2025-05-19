using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserAndDocumentManagement.Models;

[Authorize(Roles = "admin,editor")]
[ApiController]
[Route("api/[controller]")]
public class IngestionManagementController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public IngestionManagementController(ApplicationDbContext context,
        IHttpClientFactory httpClientFactory,IConfiguration configuration)
    {
        _context = context;
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    [HttpGet("status/{documentId}")]
    public async Task<IActionResult> GetStatus(int documentId)
    {
        var status = await _context.IngestionStatuses.Where(x => x.DocumentId == documentId)
            .OrderByDescending(x => x.UpdatedAt).FirstOrDefaultAsync();

        if (status == null) return NotFound("No status found for this document.");
        return Ok(status);
    }

    [HttpPost("cancel/{documentId}")]
    public async Task<IActionResult> CancelIngestion(int documentId)
    {
        //With spring boot app service
        var springUrl = _configuration["IngestionService:CancelUrl"];
        var client = _httpClientFactory.CreateClient();

        var response = await client.PostAsync($"{springUrl}/{documentId}", null);

        if (!response.IsSuccessStatusCode)
            return StatusCode((int)response.StatusCode, "Failed to cancel ingestion");

        //Without spring boot app service
        _context.IngestionStatuses.Add(new IngestionStatus
        {
            DocumentId = documentId,
            Status = "Cancelled",
            Message = "Ingestion cancelled by user",
            UpdatedAt = DateTime.UtcNow
        });

        await _context.SaveChangesAsync();
        return Ok("Ingestion cancelled");
    }

    [HttpGet("history/{documentId}")]
    public async Task<IActionResult> GetIngestionHistory(int documentId)
    {
        var history = await _context.IngestionStatuses.Where(x => x.DocumentId == documentId)
            .OrderByDescending(x => x.UpdatedAt).ToListAsync();

        return Ok(history);
    }
}