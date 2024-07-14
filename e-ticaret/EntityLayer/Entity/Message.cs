using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Entity
{
	public class Message
	{
        [Key]
        public int ID { get; set; }
        public string Content { get; set; }
        public string?  Subject { get; set; }

        public DateTime SendBy { get; set; }
        public int SendUserID { get; set; }
        public AppUser SendUser { get; set; }

        public int ReceiveUserID { get; set; }
        public AppUser ReceiveUser { get; set; }


        public bool  State { get; set; }

    }
}
