using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Entity
{
    public class category
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool  State { get; set; }
        public ICollection<urun> uruns { get; set; }

        public ICollection<CategoryFeature> CategoryFeatures { get; set; }

        public string  ImageUrl { get; set; }

        public string? Title {  get; set; }
        



    }
}
