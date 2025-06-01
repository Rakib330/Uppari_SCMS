using System.ComponentModel.DataAnnotations;

namespace Uppari_SCMS.Data
{
    public class BaseModel
    {
        public DateTime EntryTime { get; set; }
        public int EntryBy { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
