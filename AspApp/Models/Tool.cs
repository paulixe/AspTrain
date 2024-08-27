namespace AspApp.Models
{
    public class Tool
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? ImagePath { get; set; }

        public List<Workshop> AvailableAtWorkshops { get; } = new List<Workshop>();
        public List<Tutorial> UsedInTutorials { get; } = new List<Tutorial>();
        public List<Booking> Bookings { get; } = new List<Booking>();
    }
}
