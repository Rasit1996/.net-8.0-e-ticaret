using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Entity
{
    public class chart
    {
        [Key]
        public int ID { get; set; }
        public int userID { get; set; }
        public AppUser User { get; set; }
        public int urunID { get; set; }
        public urun Urun { get; set; }

        public int count { get; set; }

        public ICollection<ChartFeature> ChartFeatures { get; set; }




    }
}
