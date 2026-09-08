using System.Net.Http.Json;
using Frontend.Models;

namespace Frontend.Services;

public class TodoService(HttpClient http)
{
    public Task<List<TodoItem>?> GetAllAsync() =>
        http.GetFromJsonAsync<List<TodoItem>>("todos");

    public Task<TodoItem?> GetByIdAsync(int id) =>
        http.GetFromJsonAsync<TodoItem>($"todos/{id}");

    public async Task<TodoItem?> CreateAsync(string title)
    {
        var response = await http.PostAsJsonAsync("todos", new { title });
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TodoItem>();
    }

    public async Task<TodoItem?> UpdateAsync(int id, string title, bool isCompleted)
    {
        var response = await http.PutAsJsonAsync($"todos/{id}", new { title, isCompleted });
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TodoItem>();
    }

    public Task DeleteAsync(int id) =>
        http.DeleteAsync($"todos/{id}");
}
