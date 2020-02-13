using SelecTunes.Backend.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace SelecTunes.Backend.Data
{
    public class ApplicationContext : IdentityDbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        public DbSet<HostUser> HostUsers { get; set; }

        public DbSet<Party> Parties { get; set; }

        public DbSet<User> BannedUsers { get; set; }
    }
}
