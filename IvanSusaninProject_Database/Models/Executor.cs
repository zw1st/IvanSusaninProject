namespace IvanSusaninProject_Database.Models;

public class Executor
{
    public required string Id { get; set; } = Guid.NewGuid().ToString();

    public required string Login { get; set; }

    public required string Password { get; set; }

    public string? Email { get; set; }
}
