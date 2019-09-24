using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Entities.Shop;

namespace Models.Configurations
{
	public class CategoryConfiguration : IEntityTypeConfiguration<Category>
	{
		public void Configure(EntityTypeBuilder<Category> builder)
		{
			builder.ToTable("Categories");

			builder.HasData
			(
				new Category
				{
					Id = Guid.NewGuid().ToString(),
					Name = "Test Cat 1",
				},
				new Category
				{
					Id = Guid.NewGuid().ToString(),
					Name = "Test Cat 2"
				},
                new Category
				{
					Id = Guid.NewGuid().ToString(),
					Name = "Test Cat 3"
				}
			);
		}
	}
}