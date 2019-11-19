using ErrorIt.Api.Data.Models;
using ErrorIt.Api.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ErrorIt.Data
{
	public class AppDbContext : DbContext
	{
		public DbSet<ApplicationGroup> ApplicationGroups { get; set; }
		public DbSet<Application> Applications { get; set; }
		public DbSet<ErrorTemplate> ErrorTemplates { get; set; }

		public AppDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }
			
		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.Entity<Application>().HasIndex(x => new { x.ApplicationGroupId, x.Name }).IsUnique();
			builder.Entity<ApplicationGroup>().HasIndex(x => x.Name).IsUnique();
			builder.Entity<ErrorTemplate>().HasIndex(x => new { x.ApplicationErrorCode , x.ApplicationId }).IsUnique();
		}

		public override int SaveChanges()
		{
			SetModifiers();
			return base.SaveChanges();
		}

		public override int SaveChanges(bool acceptAllChangesOnSuccess)
		{
			SetModifiers();
			return base.SaveChanges(acceptAllChangesOnSuccess);
		}

		public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			SetModifiers();
			return await base.SaveChangesAsync();
		}

		private void SetModifiers()
		{
			foreach(var entity in this.ChangeTracker.Entries<IBaseEntity>().Where(x => x.State == EntityState.Added || x.State == EntityState.Modified || x.State == EntityState.Deleted))
			{
				entity.Entity.CreatedOn = entity.State == EntityState.Added ? DateTime.UtcNow : entity.Entity.CreatedOn;
				entity.Entity.UpdatedOn = DateTime.UtcNow;
				entity.Entity.Active = entity.State != EntityState.Deleted;
			}
		}
	}
}
