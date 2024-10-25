using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PuanConnect.Dtos.Category;
public class CategoryDto 
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
}