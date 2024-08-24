namespace AspApp.Models
{
    public class Tutorial
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public List<Tool> RequiredTools { get; } = new List<Tool>();
    }
}
