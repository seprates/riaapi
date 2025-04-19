using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RIA.API.Models;

public class Customer
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }
    [Required]
    public required string LastName { get; set; }
    [Required]
    public required string FirstName { get; set; }
    [Required]
    public required int Age { get; set; }
    public DateTime CreatedDateUtc { get; set; } = DateTime.UtcNow;
}