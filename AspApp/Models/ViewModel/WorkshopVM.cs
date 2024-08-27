namespace AspApp.Models.ViewModel
{
    public class WorkshopVM
    {
        public Workshop Workshop { get; set; }
        public List<ToolCheckbox> ToolCheckboxes { get; set; }
        public string BookingDate { get; set; }
    }
}
