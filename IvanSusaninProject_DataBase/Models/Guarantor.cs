
namespace IvanSusaninProject_DataBase.Models;

public class Guarantor
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public required string Login { get;  set; }

    public required string Password { get;  set; }

    public string? Email { get;  set; }

}