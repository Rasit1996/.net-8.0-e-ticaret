using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Entity
{
	public class salesDetails
	{
		public int salesID { get; set; }
		public sales sales { get; set; }

		public int urunID { get; set; }

		public urun urun { get; set; }

		public int FeatureID { get; set; }
		public Feature Feature { get; set; }

        public int GroupID { get; set; }

        public double price { get; set; }
        public double? discount { get; set; }
        public int pience { get; set; }

        public bool state { get; set; }

    }
}
