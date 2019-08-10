using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Models
{
    public class DbContext : IdentityDbContext<ApplicationUser>
    {
        public DbContext(DbContextOptions<DbContext> options) : base(options)
        {

        }

        public override int SaveChanges()
        {
            UpdateDateModified();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            UpdateDateModified();
            return await base.SaveChangesAsync();
        }

        private void UpdateDateModified()
        {
            var entitiesUsers = ChangeTracker.Entries().Where(x => x.Entity is ApplicationUser && (x.State == EntityState.Modified));

            foreach (var e in entitiesUsers)
            {
                ((ApplicationUser)e.Entity).DateModified = DateTime.UtcNow;
            }

            var entities = ChangeTracker.Entries().Where(x => x.Entity is BaseEntity && (x.State == EntityState.Modified));

            foreach (var e in entities)
            {
                ((BaseEntity)e.Entity).DateModified = DateTime.UtcNow;
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customization after calling base.OnModelCreating(builder);

            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserToken");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");
            modelBuilder.Entity<ApplicationUser>().ToTable("Users");

            // default values --
            modelBuilder.Entity<ApplicationUser>()
                .Property(e => e.DateCreated).HasDefaultValueSql("getutcdate()").ValueGeneratedOnAdd();
        }
    }
}
