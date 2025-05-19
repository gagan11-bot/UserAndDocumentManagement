using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserAndDocumentManagement.DTO;
using UserAndDocumentManagement.Models;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class DocumentsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _env;

    public DocumentsController(ApplicationDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload([FromForm] FileUploadModel model)
    {
        if (model.File == null || model.File.Length == 0)
            return BadRequest("No file uploaded");

        var uploadsFolder = Path.Combine(_env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads"));

        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        //var fileName = Guid.NewGuid() + Path.GetExtension(model.File.FileName);
        var fileName = Guid.NewGuid() + "-" + Path.GetExtension(model.File.FileName) + "-" + Path.GetFileName(model.File.FileName);
        var filePath = Path.Combine(uploadsFolder, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await model.File.CopyToAsync(stream);
        }

        var document = new Document
        {
            Title = model.Title,
            FilePath = fileName,
            UploadedBy = User.Identity?.Name ?? "unknown",
            UploadedAt = DateTime.UtcNow
        };

        _context.Documents.Add(document);
        await _context.SaveChangesAsync();

        return Ok(document);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var documents = await _context.Documents.ToListAsync();
        return Ok(documents);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var doc = await _context.Documents.FindAsync(id);
        if (doc == null)
            return NotFound();

        return Ok(doc);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Document updated)
    {
        var doc = await _context.Documents.FindAsync(id);
        if (doc == null)
            return NotFound();

        doc.Title = updated.Title;
        await _context.SaveChangesAsync();

        return Ok(doc);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var doc = await _context.Documents.FindAsync(id);
        if (doc == null)
            return NotFound();

        var filePath = Path.Combine(_env.WebRootPath ?? "Uploads", doc.FilePath);
        if (System.IO.File.Exists(filePath))
            System.IO.File.Delete(filePath);

        _context.Documents.Remove(doc);
        await _context.SaveChangesAsync();

        return Ok("Deleted");
    }
}