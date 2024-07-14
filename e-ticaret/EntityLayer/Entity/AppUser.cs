using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace EntityLayer.Entity
{
    public class AppUser:IdentityUser<int>
    {
        public ICollection<chart> Charts { get; set; }

        public ICollection<Message> ReceivedMessages { get; set; }

        public ICollection<Message> SentMessages { get; set; }

        public string  SurName { get; set; }

        

    }
}
