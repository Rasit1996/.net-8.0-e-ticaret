using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Entity
{
    public class UrunFeature
    {
        public int urunID { get; set; }
        public int featureID { get; set; }
        public urun Urun { get; set; }
        public Feature Feature { get; set; }

    }
}
