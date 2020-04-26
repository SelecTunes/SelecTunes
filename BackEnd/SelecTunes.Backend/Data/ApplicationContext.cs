using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SelecTunes.Backend.Models;
using System;

namespace SelecTunes.Backend.Data
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        public DbSet<Party> Parties { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Entity<Party>().HasMany(u => u.PartyMembers).WithOne(d => d.Party).HasForeignKey(d => d.PartyId);
            builder.Entity<Party>().HasOne(e => e.PartyHost).WithOne().HasForeignKey<Party>(e => e.PartyHostId);

            base.OnModelCreating(builder);
        }
    }
}
