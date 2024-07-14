using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Entity
{
    public class urun
    {
        [Key]
        public int ID { get; set; }
        public string  Name { get; set; }
        public string?  Description { get; set; }
        public int? Stock { get; set; }
        public double Price { get; set; }
        public float? Discount { get; set; }

        public int  categoryID { get; set; }
        public category category { get; set; }
        public ICollection<salesDetails> SalesDetails { get; set; }

        public ICollection<chart> Charts { get; set; }

        public ICollection<UrunFeature> UrunFeatures { get; set; }

        public ICollection<urunImage> urunImages { get; set; }

       

        public bool State { get; set; }
        




    }
}
