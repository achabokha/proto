using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Embily.Models
{
    public class EmbilyDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<AffiliateEmail> AffiliateEmails { get; set; }

        public DbSet<AffiliateToken> AffiliateTokens { get; set; }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<CryptoAddress> CryptoAddresses { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<Application> Applications { get; set; }

        public DbSet<Document> Documents { get; set; }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<Configuration> Configurations { get; set; }

        public DbSet<CardOrder> CardOrders { get; set; }

        public DbSet<Program> Programs { get; set; }


        public EmbilyDbContext(DbContextOptions<EmbilyDbContext> options) : base(options)
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

            // sequences-- 
            modelBuilder.HasSequence<Int64>("UserNumbers")
                .StartsAt(1000001009)
                .IncrementsBy(1);

            modelBuilder.Entity<ApplicationUser>()
                .Property(e => e.UserNumber)
                .HasDefaultValueSql("NEXT VALUE FOR UserNumbers");

            modelBuilder.HasSequence<Int64>("TransactionNumbers")
                .StartsAt(10000)
                .IncrementsBy(1);

            modelBuilder.Entity<Transaction>()
                .Property(e => e.TransactionNumber)
                .HasDefaultValueSql("NEXT VALUE FOR TransactionNumbers");

            // indexes on the Numbers 
            modelBuilder.Entity<ApplicationUser>()
                .HasIndex(e => e.UserNumber)
                .IsUnique();

            modelBuilder.Entity<Application>()
                .HasIndex(e => e.ApplicationNumber)
                .IsUnique();

            modelBuilder.Entity<Account>()
                .HasIndex(e => e.AccountNumber)
                .IsUnique();

            modelBuilder.Entity<Transaction>()
                .HasIndex(e => e.TransactionNumber)
                .IsUnique();

            modelBuilder.Entity<AffiliateEmail>()
                .HasIndex(e => e.NormalizedEmail)
                .IsUnique();

            modelBuilder.Entity<AffiliateToken>()
                .HasIndex(e => e.NormalizedToken)
                .IsUnique();

            // default values --
            modelBuilder.Entity<Account>()
                .Property(e => e.DateCreated).HasDefaultValueSql("getutcdate()").ValueGeneratedOnAdd();

            modelBuilder.Entity<Address>()
                .Property(e => e.DateCreated).HasDefaultValueSql("getutcdate()").ValueGeneratedOnAdd();

            modelBuilder.Entity<Application>()
                .Property(e => e.DateCreated).HasDefaultValueSql("getutcdate()").ValueGeneratedOnAdd();

            modelBuilder.Entity<ApplicationUser>()
                .Property(e => e.DateCreated).HasDefaultValueSql("getutcdate()").ValueGeneratedOnAdd();

            modelBuilder.Entity<Configuration>()
               .Property(e => e.DateCreated).HasDefaultValueSql("getutcdate()").ValueGeneratedOnAdd();

            modelBuilder.Entity<CryptoAddress>()
                .Property(e => e.DateCreated).HasDefaultValueSql("getutcdate()").ValueGeneratedOnAdd();

            modelBuilder.Entity<Document>()
                .Property(e => e.DateCreated).HasDefaultValueSql("getutcdate()").ValueGeneratedOnAdd();

            modelBuilder.Entity<Transaction>()
                .Property(e => e.DateCreated).HasDefaultValueSql("getutcdate()").ValueGeneratedOnAdd();

            modelBuilder.Entity<AffiliateEmail>()
                .Property(e => e.DateCreated).HasDefaultValueSql("getutcdate()").ValueGeneratedOnAdd();

            modelBuilder.Entity<AffiliateToken>()
                .Property(e => e.DateCreated).HasDefaultValueSql("getutcdate()").ValueGeneratedOnAdd();

            modelBuilder.Entity<CardOrder>()
                .Property(e => e.DateCreated).HasDefaultValueSql("getutcdate()").ValueGeneratedOnAdd();

            modelBuilder.Entity<Program>()
                .Property(e => e.DateCreated).HasDefaultValueSql("getutcdate()").ValueGeneratedOnAdd();
        }
    }
}
