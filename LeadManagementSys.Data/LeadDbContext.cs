using LeadManagementSys.Models.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace LeadManagementSys.Data
{
    public class LeadDbContext : IdentityDbContext<ApplicationUser>

    {
        public LeadDbContext(DbContextOptions<LeadDbContext> options)
            : base(options)
        {
        }
        public DbSet<Lead> Leads { get; set; }
        public DbSet<LeadRemark> LeadRemarks { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            
            builder.Entity<ApplicationUser>()
                .HasDiscriminator<string>("UserType")
                .HasValue<Admin>("Admin")
                .HasValue<Manager>("Manager")
                .HasValue<Agent>("Agent");

            
            builder.Entity<Agent>()
                .HasOne(a => a.Manager)
                .WithMany(m => m.Agents)
                .HasForeignKey(a => a.ManagerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Manager>()
                .HasOne(m => m.Admin)
                .WithMany(a => a.Managers)
                .HasForeignKey(m => m.AdminId)
                .OnDelete(DeleteBehavior.Restrict);


            builder.Entity<Lead>()
                .HasMany(l => l.Remarks)
                .WithOne(r => r.Lead)
                .HasForeignKey(r => r.LeadId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
