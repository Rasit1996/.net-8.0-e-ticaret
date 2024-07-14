using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Entity
{
    public class Feature
    {
        [Key]
        public int ID { get; set; }

        public string Name { get; set; }
        public string? description { get; set; }

        public string?  property { get; set; }

        public bool  state { get; set; }

        public ICollection<UrunFeature> UrunFeatures { get; set; }

        public ICollection<CategoryFeature> CategoryFeatures { get; set; }
        public ICollection<ChartFeature> ChartFeatures { get; set; }

		public ICollection<salesDetails> SalesDetails { get; set; }


		public string Title { get; set; }


    }
}
