using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models.Entities;
using Models.Entities.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Models.Configurations;

namespace Models
{
	public class DbContext : IdentityDbContext<ApplicationUser>
	{
		public DbSet<ChatMessage> ChatMessages { get; set; }
		public DbSet<Participant> Participants { get; set; }
		public DbSet<ChatGroup> ChatGroups { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<Category> Categories { get; set; }

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

			//modelBuilder.ApplyConfiguration(new CategoryConfiguration());
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

			List<dynamic> categories = new List<dynamic>(){
					new
					{
						Id = "1",
						Name = "Test Cat 1",
					},
					new
					{
						Id = "2",
						Name = "Test Cat 2"
					},
					new
					{
						Id = "3",
						Name = "Test Cat 3"
					}};

			List<Object> products = new List<Object>() {
				new {
					Id = "1",
					Price = 0.12m,
					ImageUrl = "product1.jpg",
					CategoryId = categories[0].Id
				},
				new {
					Id = "2",
					Price = 10.12m,
					ImageUrl = "product2.jpg",
					CategoryId = categories[0].Id
				},
				new {
					Id = "3",
					Price = 1231230.12m,
					ImageUrl = "product3.jpg",
					CategoryId = categories[1].Id
				}
			};

			modelBuilder.Entity<Category>(c =>
			{
				c.HasData(categories);
				c.HasMany(d => d.Products);
			});

			modelBuilder.Entity<Product>(c =>
			{
				
				c.HasOne(d => d.Category);
				c.HasData(products);
			});

			modelBuilder.Entity<Order>(c =>
			{
				c.HasMany(d => d.Products);
			});





			// default values --
			modelBuilder.Entity<ApplicationUser>()
				.Property(e => e.DateCreated).HasDefaultValueSql("getutcdate()").ValueGeneratedOnAdd();
		}
	}
}
