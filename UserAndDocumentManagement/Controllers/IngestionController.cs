using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using UserAndDocumentManagement.Models;

[Authorize(Roles = "admin,editor")]
[ApiController]
[Route("api/[controller]")]
public class IngestionController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly ApplicationDbContext _context;

    public IngestionController(IHttpClientFactory httpClientFactory, IConfiguration configuration,
        ApplicationDbContext context)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _context = context;
    }

    [HttpPost("trigger")]
    public async Task<IActionResult> TriggerIngestion([FromBody] IngestionRequestModel model)
    {
        //With Spring Boot app service code
        var client = _httpClientFactory.CreateClient();

        var springBootUrl = _configuration["IngestionService:Url"];

        var request = new HttpRequestMessage(HttpMethod.Post, springBootUrl);
        request.Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");

        var response = await client.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            var status = new IngestionStatus
            {
                DocumentId = model.DocumentId,
                Status = "Ingesting",
                Message = "Ingestion started.",
                UpdatedAt = DateTime.UtcNow
            };

            _context.IngestionStatuses.Add(status);
            await _context.SaveChangesAsync();

            return Ok("Ingestion triggered.");
        }

        return StatusCode((int)response.StatusCode, "Ingestion failed.");


        //// Without Spring boot app service code
        //var status = new IngestionStatus
        //{
        //    DocumentId = model.DocumentId,
        //    Status = "Ingesting",
        //    Message = "Ingestion started.",
        //    UpdatedAt = DateTime.UtcNow
        //};

        //_context.IngestionStatuses.Add(status);
        //await _context.SaveChangesAsync();

        //return Ok("Ingestion triggered.");
    }
}