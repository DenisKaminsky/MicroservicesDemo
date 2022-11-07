using System.ComponentModel.DataAnnotations;

namespace CommandsService.Data.Models;

public class Platform
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int ExternalId { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    public IEnumerable<Command>? Commands { get; set; }
}