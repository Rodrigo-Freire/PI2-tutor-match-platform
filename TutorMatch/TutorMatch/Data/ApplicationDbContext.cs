using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TutorMatch.Models;

namespace TutorMatch.Data
	{
	public class ApplicationDbContext : IdentityDbContext<User>
		{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
			{
			}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
			{
			base.OnModelCreating(modelBuilder);

			// Configurando os tipos de dados para o PostgreSQL
			modelBuilder.Entity<User>()
				.Property(u => u.Id)
				.HasColumnType("varchar(450)");

			modelBuilder.Entity<User>()
				.Property(u => u.Email)
				.HasColumnType("varchar(256)");

			modelBuilder.Entity<User>()
				.Property(u => u.NormalizedEmail)
				.HasColumnType("varchar(256)");

			modelBuilder.Entity<IdentityRole>()
				.Property(r => r.Id)
				.HasColumnType("varchar(450)");

			modelBuilder.Entity<IdentityRole>()
				.Property(r => r.Name)
				.HasColumnType("varchar(256)");

			modelBuilder.Entity<IdentityRole>()
				.Property(r => r.NormalizedName)
				.HasColumnType("varchar(256)");

			modelBuilder.Entity<IdentityUserClaim<string>>()
				.Property(c => c.Id)
				.HasColumnType("varchar(450)");

			modelBuilder.Entity<IdentityUserRole<string>>()
				.Property(ur => ur.UserId)
				.HasColumnType("varchar(450)");

			modelBuilder.Entity<IdentityUserRole<string>>()
				.Property(ur => ur.RoleId)
				.HasColumnType("varchar(450)");

			modelBuilder.Entity<IdentityUserLogin<string>>()
				.Property(l => l.LoginProvider)
				.HasColumnType("varchar(128)");

			modelBuilder.Entity<IdentityUserLogin<string>>()
				.Property(l => l.ProviderKey)
				.HasColumnType("varchar(128)");

			modelBuilder.Entity<IdentityUserToken<string>>()
				.Property(t => t.UserId)
				.HasColumnType("varchar(450)");
			}
		}
	}
