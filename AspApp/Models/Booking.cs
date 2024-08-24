using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AspApp.Models
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }

        public string? CustomerId { get; set; }
        public IdentityUser? Customer { get; set; }
        public int WorkshopId { get; set; }
        public Workshop? Workshop { get; set; }
        public List<Tool> ItemsBooked { get; } = new List<Tool>();
    }
}
