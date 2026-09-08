using System.Security.Claims;
using Backend.Data;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class TodosController(AppDbContext db) : ControllerBase
{
    private string UserId =>
        User.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? throw new InvalidOperationException("User is not authenticated.");

    // GET api/todos
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var todos = await db.Todos
            .Where(t => t.UserId == UserId)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();

        return Ok(todos);
    }

    // GET api/todos/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var todo = await db.Todos
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == UserId);

        return todo is null ? NotFound() : Ok(todo);
    }

    // POST api/todos
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TodoRequest request)
    {
        var todo = new TodoItem
        {
            Title  = request.Title,
            UserId = UserId,
        };

        db.Todos.Add(todo);
        await db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = todo.Id }, todo);
    }

    // PUT api/todos/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] TodoRequest request)
    {
        var todo = await db.Todos
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == UserId);

        if (todo is null) return NotFound();

        todo.Title       = request.Title;
        todo.IsCompleted = request.IsCompleted;

        await db.SaveChangesAsync();
        return Ok(todo);
    }

    // DELETE api/todos/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var todo = await db.Todos
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == UserId);

        if (todo is null) return NotFound();

        db.Todos.Remove(todo);
        await db.SaveChangesAsync();
        return NoContent();
    }
}

// ── Request DTO ───────────────────────────────────────────────────────────────
public record TodoRequest(string Title, bool IsCompleted = false);
