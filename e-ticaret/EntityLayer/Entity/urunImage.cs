using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Entity
{
    public class urunImage
    {
        [Key]
        public int ID { get; set; }

        public int urunID { get; set; }
        public urun Urun { get; set; }
        public string  ImageUrl { get; set; }

    }
}
