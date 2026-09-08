using System.ComponentModel.DataAnnotations;

namespace Backend.Models;

public class TodoItem
{
    public int Id { get; set; }

    [Required, MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    public bool IsCompleted { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Tie each todo to the owning user (nullable = global todos)
    public string? UserId { get; set; }
}
