using EntityLayer.Entity;

namespace e_ticaret.Models
{
    public class SalesDetailsViewModel
    {
        public DateTime SellBy { get; set; }
        public int SalesID { get; set; }
        public ICollection<salesDetails>  SalesDetails { get; set; }
    }
}
