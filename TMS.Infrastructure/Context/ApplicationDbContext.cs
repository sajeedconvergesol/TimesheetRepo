using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using TMS.Core;

namespace TMS.Infrastructure.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
    {
        //public ApplicationDbContext(){}
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
          : base(options)
        {
        }

        #region Entities
        public DbSet<Category> Categories { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceDetails> InvoiceDetails { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectDocuments> ProjectDocuments { get; set; }
        public DbSet<TaskAssignment> TaskAssignments { get; set; }
        public DbSet<Tasks> Task { get; set; }
        public DbSet<TimeSheetApprovals> TimeSheetApprovals { get; set; }
        public DbSet<TimeSheetDetails> TimeSheetDetails { get; set; }
        public DbSet<TimeSheetMaster> TimeSheetMaster { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder builder)
        {
            
            builder.Entity<ApplicationRole>().HasData(new ApplicationRole
            {
                Id = 1,
                Name = "Managers",
                NormalizedName = "Managers"
            });
            builder.Entity<ApplicationRole>().HasData(new ApplicationRole
            {
                Id = 2,
                Name = "Developers",
                NormalizedName = "Developers"
            });
            var hasher = new PasswordHasher<ApplicationUser>();
            builder.Entity<ApplicationUser>().HasData(new ApplicationUser
            {
                Id = 1,
                UserName = "admin",
                NormalizedUserName = "admin",
                FirstName = "Admin",
                LastName = "Admin",
                IsActive = true,
                Email = "admin@local",
                NormalizedEmail = "admin@local",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "Admin@123"),
                SecurityStamp = Guid.NewGuid().ToString("D"),
            });
            builder.Entity<ApplicationUser>().HasData(new ApplicationUser
            {
                Id = 2,
                UserName = "Manager",
                NormalizedUserName = "Manager",
                FirstName = "Manager",
                LastName = "Manager",
                IsActive = true,
                Email = "manager@local",
                NormalizedEmail = "manager@local",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "Manager@123"),
                SecurityStamp = Guid.NewGuid().ToString("D"),
            });
            base.OnModelCreating(builder);

        }
    }
}
