using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TutorMatch.Data;
using TutorMatch.Models; // Certifique-se de que a classe User est� aqui
using Microsoft.Extensions.Configuration;

namespace TutorMatch;
public class Program
	{
	public static async Task Main(string[] args)
		{
		var builder = WebApplication.CreateBuilder(args);

		// Adicionar suporte para vari�veis de ambiente
		builder.Configuration
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
			.AddEnvironmentVariables(); // Isso permite usar as vari�veis de ambiente

		// Recuperar a connection string de uma vari�vel de ambiente ou do appsettings.json
		var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
			?? $"Host={Environment.GetEnvironmentVariable("DB_HOST")};" +
			   $"Username={Environment.GetEnvironmentVariable("DB_USER")};" +
			   $"Password={Environment.GetEnvironmentVariable("DB_PASSWORD")};" +
			   $"Database={Environment.GetEnvironmentVariable("DB_NAME")};";

		if (string.IsNullOrEmpty(connectionString))
			{
			throw new InvalidOperationException("Connection string 'DefaultConnection' or environment variable 'DB_CONNECTION_STRING' not found.");
			}

		// Configurar o contexto de banco de dados
		builder.Services.AddDbContext<ApplicationDbContext>(options =>
			options.UseNpgsql(connectionString)); // UseNpgsql se estiver usando PostgreSQL

		builder.Services.AddDatabaseDeveloperPageExceptionFilter();

		// Configurar o Identity para usar a classe User
		builder.Services.AddIdentity<User, IdentityRole>(options =>
		{
			// Configura��es de senha
			options.Password.RequireDigit = true;
			options.Password.RequiredLength = 6;
			options.Password.RequireNonAlphanumeric = false;
			options.Password.RequireUppercase = false;
			options.Password.RequireLowercase = true;

			// Configura��es de bloqueio
			options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
			options.Lockout.MaxFailedAccessAttempts = 5;
			options.Lockout.AllowedForNewUsers = true;

			// Configura��es de usu�rio
			options.User.RequireUniqueEmail = true;
		})
		.AddEntityFrameworkStores<ApplicationDbContext>()
		.AddDefaultTokenProviders();

		// Configurar autentica��o via cookie
		builder.Services.ConfigureApplicationCookie(options =>
		{
			options.LoginPath = "/Account/Login";
			options.AccessDeniedPath = "/Account/AccessDenied";
		});

		// Adicione os servi�os necess�rios para Razor Pages e MVC
		builder.Services.AddRazorPages();
		builder.Services.AddControllersWithViews();

		var app = builder.Build();

		// Configure o pipeline de requisi��es HTTP.
		if (app.Environment.IsDevelopment())
			{
			app.UseMigrationsEndPoint();
			}
		else
			{
			app.UseExceptionHandler("/Home/Error");
			app.UseHsts();
			}

		app.UseHttpsRedirection();
		app.UseStaticFiles();

		app.UseRouting();

		// Adicionar autentica��o e autoriza��o
		app.UseAuthentication();
		app.UseAuthorization();

		app.MapRazorPages();
		app.MapControllerRoute(
			name: "default",
			pattern: "{controller=Home}/{action=Index}/{id?}");

		// Mapeando a rota para o controlador de teste de banco de dados
		app.MapControllerRoute(
			name: "dbTest",
			pattern: "DatabaseTest",
			defaults: new { controller = "DatabaseTest", action = "Index" });

		// Mapeando a rota padr�o para controladores MVC
		app.MapControllerRoute(
			name: "default",
			pattern: "{controller=Home}/{action=Index}/{id?}");

		// Iniciar o aplicativo
		await app.RunAsync();
		}
	}
