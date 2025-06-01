namespace Uppari_SCMS.Data.Models
{
    public class Vendor : BaseModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
    }
}
