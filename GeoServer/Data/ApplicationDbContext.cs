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

            builder.Entity<ZonalStatKATO>()
                .HasIndex(z => z.DayOfYear);

            //modelBuilder.Entity<Person>()
            //.HasIndex(p => new { p.FirstName, p.LastName });
        }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }

        public DbSet<GeoServer.Models.ModisSource> ModisSource { get; set; }

        public DbSet<GeoServer.Models.ModisProduct> ModisProduct { get; set; }

        public DbSet<GeoServer.Models.ModisSpan> ModisSpan { get; set; }

        public DbSet<GeoServer.Models.ModisDataSet> ModisDataSet { get; set; }

        public DbSet<GeoServer.Models.CoordinateSystems> CoordinateSystems { get; set; }

        public DbSet<GeoServer.Models.Log> Log { get; set; }

        public DbSet<GeoServer.Models.ZonalStatKATO> ZonalStatKATO { get; set; }

        public DbSet<GeoServer.Models.ZonalStatPast> ZonalStatPast { get; set; }

        public DbSet<GeoServer.Models.PasClass> PasClass { get; set; }

        public DbSet<GeoServer.Models.PasOtdel> PasOtdel { get; set; }

        public DbSet<GeoServer.Models.PasSubtype> PasSubtype { get; set; }

        public DbSet<GeoServer.Models.PasGroup> PasGroup { get; set; }

        public DbSet<GeoServer.Models.PasRecom> PasRecom { get; set; }

        public DbSet<GeoServer.Models.Pasture> Pasture { get; set; }
    }

    //public class MyContext : DbContext
    //{
    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //        => optionsBuilder.UseNpgsql(Startup.Configuration["ConnectionStrings:DefaultConnection"]);

    //    public DbSet<GeoServer.Models.ModisSource> ModisSource { get; set; }

    //    public DbSet<GeoServer.Models.ModisProduct> ModisProduct { get; set; }

    //    public DbSet<GeoServer.Models.ModisSpan> ModisSpan { get; set; }

    //    public DbSet<GeoServer.Models.ModisDataSet> ModisDataSet { get; set; }

    //    public DbSet<GeoServer.Models.CoordinateSystems> CoordinateSystems { get; set; }

    //    public DbSet<GeoServer.Models.Log> Log { get; set; }
    //}
}
