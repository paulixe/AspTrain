using System.ComponentModel.DataAnnotations;

namespace AspApp.Models
{
    public class PostIt
    {
        public int Id { get; set; }
        [Required]
        public string? Description { get; set; }
    }
}
