using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using GeoServer.Models;

namespace GeoServer.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }

        public DbSet<GeoServer.Models.ModisSource> ModisSource { get; set; }

        public DbSet<GeoServer.Models.ModisProduct> ModisProduct { get; set; }

        public DbSet<GeoServer.Models.ModisSpan> ModisSpan { get; set; }

        public DbSet<GeoServer.Models.ModisDataSet> ModisDataSet { get; set; }

        public DbSet<GeoServer.Models.CoordinateSystems> CoordinateSystems { get; set; }
    }
}
