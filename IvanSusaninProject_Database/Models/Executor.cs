using IvanSusaninProject_Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_Database.Models;

internal class Executor
{
    public required string Id { get; set; } = Guid.NewGuid().ToString();

    public required string Login { get; set; }

    public required string Password { get; set; }

    public string? Email { get; set; }
}
