using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Entity
{
	public class ChartFeature
	{
		public int chartID { get; set; }
		public chart Chart { get; set; }
		public int featureID { get; set; }
		public Feature Feature { get; set; }
	}
}
