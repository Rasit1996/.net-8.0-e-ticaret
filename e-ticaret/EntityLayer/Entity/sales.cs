using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Entity
{
    public class sales
    {
        [Key]
        public int ID { get; set; }
        public int userID { get; set; }
        public AppUser User { get; set; }
        public DateTime SellBy { get; set; }
        public ICollection<salesDetails> SalesDetails { get; set; }
        public bool  state { get; set; }

    }
}
