namespace AspApp.Models
{
    public class Workshop
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public List<Booking> Bookings { get; } = new List<Booking>();
    }
}
