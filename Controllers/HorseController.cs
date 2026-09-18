using HorseManager.Api.Data;
using HorseManager.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace HorseManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HorsesController : ControllerBase
{
    private readonly HorseDbContext _context;
    private readonly IWebHostEnvironment _env;

    public HorsesController(HorseDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Horse>>> GetHorses()
    {
        return await _context.Horses.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Horse>> GetHorse(int id)
    {
        var horse = await _context.Horses.FindAsync(id);
        if (horse == null)
            return NotFound();
        return horse;
    }

    [HttpPost]
    public async Task<ActionResult<Horse>> PostHorse(CreateHorseDto dto)
    {
        var horse = new Horse
        {
            Name = dto.Name,
            Breed = dto.Breed,
            BirthYear = dto.BirthYear,
            Breeder = dto.Breeder,
            Owner = dto.Owner,
            Notes = dto.Notes,
            IsCurrent = dto.IsCurrent
        };

        _context.Horses.Add(horse);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetHorse), new { id = horse.Id }, horse);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutHorse(int id, UpdateHorseDto dto)
    {
        var horse = await _context.Horses.FindAsync(id);
        if (horse == null)
            return NotFound();

        horse.Name = dto.Name;
        horse.Breed = dto.Breed;
        horse.BirthYear = dto.BirthYear;
        horse.Breeder = dto.Breeder;
        horse.Owner = dto.Owner;
        horse.Notes = dto.Notes;
        horse.IsCurrent = dto.IsCurrent;

        await _context.SaveChangesAsync();
        return Ok(horse);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteHorse(int id)
    {
        var horse = await _context.Horses.FindAsync(id);
        if (horse == null)
            return NotFound();

        _context.Horses.Remove(horse);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPost("{id}/image")]
    public async Task<IActionResult> UploadImage(int id, IFormFile file)
    {
        var horse = await _context.Horses.FindAsync(id);
        if (horse == null)
            return NotFound();

        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded");

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
        var extension = Path.GetExtension(file.FileName).ToLower();
        if (!allowedExtensions.Contains(extension))
            return BadRequest("Invalid file type. Only jpg, png, gif allowed.");

        if (file.Length > 5 * 1024 * 1024) // 5 MB
            return BadRequest("File too large. Maximum 5 MB.");

        var uploadsDir = Path.Combine(_env.WebRootPath, "uploads");
        Directory.CreateDirectory(uploadsDir);

        var fileName = $"{horse.Id}_{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine(uploadsDir, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        horse.ImagePath = $"/uploads/{fileName}";
        await _context.SaveChangesAsync();

        return Ok(new { imagePath = horse.ImagePath });
    }
}

public class CreateHorseDto
{
    public string Name { get; set; } = string.Empty;
    public string Breed { get; set; } = string.Empty;
    public int BirthYear { get; set; }
    public string? Breeder { get; set; }
    public string Owner { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public bool IsCurrent { get; set; } = true;


}

public class UpdateHorseDto
{
    public string Name { get; set; } = string.Empty;
    public string Breed { get; set; } = string.Empty;
    public int BirthYear { get; set; }
    public string? Breeder { get; set; }
    public string Owner { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public bool IsCurrent { get; set; } = true;


}
