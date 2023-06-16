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
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        //public ApplicationDbContext(){}
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
          : base(options)
        {
        }

        #region Entities
        public DbSet<InvoiceDetails> InvoiceDetail { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = "1",
                Name = "Admin",
                NormalizedName = "Admin"
            });
            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = "2",
                Name = "Managers",
                NormalizedName = "Managers"
            });
            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = "3",
                Name = "Developers",
                NormalizedName = "Developers"
            });
            var hasher = new PasswordHasher<ApplicationUser>();
            builder.Entity<ApplicationUser>().HasData(new ApplicationUser
            {
                Id = "1",
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
                Id = "2",
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
