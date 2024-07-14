using DataAccessLayer.Connections;
using EntityLayer.Entity;

namespace e_ticaret.Models
{
    public class ProductViewModel
    {
        
        public string?  Name { get; set; }
        public string? Description { get; set; }
        public int? Stock { get; set; }
        public double? Price { get; set; }
        public float? Discount { get; set; }
        public int?  CategoryID { get; set; }

       

        

    }
}
