using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PartyProduct_Exercise_03.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PartyProduct_Exercise_03.Data
{
    public class PartyProductMVCContext : IdentityDbContext<ApplicationUser>
    {
        public PartyProductMVCContext(DbContextOptions<PartyProductMVCContext> options)
            : base(options)
        {
        }
        public DbSet<Party> Party { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<AssignParty> AssignParty { get; set; }
        public DbSet<ProductRate> ProductRate { get; set; }
        public DbSet<Invoice> Invoice { get; set; }
    }
}
