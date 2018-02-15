using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace login_v1.Models
{
    public class login_v1Context : DbContext
    {
        public login_v1Context (DbContextOptions<login_v1Context> options)
            : base(options)
        {
        }

        public DbSet<login_v1.Models.User> User { get; set; }
    }
}
