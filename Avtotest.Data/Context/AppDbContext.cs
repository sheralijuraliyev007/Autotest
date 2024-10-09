using Avtotest.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avtotest.Data.Context
{
    public class AppDbContext:IdentityDbContext<IdentityUser>
    {

        public AppDbContext(DbContextOptions<AppDbContext> options):base(options) { 
            
        }

        public DbSet<CustomUser> CustomUsers { get; set; }
        public DbSet<Result> Results { get; set; }

    }
}
