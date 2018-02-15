using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace login_v1.Models
{
    public class User
    {
		public int ID { get; set; }
		public string username { get; set; }
		public string password { get; set; }
        public string salt { get; set; }
    }
}
