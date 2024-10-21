using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TutorMatch.Data;
using TutorMatch.Models; // Certifique-se de que a classe User está aqui

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
	?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Configurar o Identity para usar a classe User
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
	// Configurações de senha
	options.Password.RequireDigit = true;
	options.Password.RequiredLength = 6;
	options.Password.RequireNonAlphanumeric = false;
	options.Password.RequireUppercase = false;
	options.Password.RequireLowercase = true;

	// Configurações de bloqueio
	options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
	options.Lockout.MaxFailedAccessAttempts = 5;
	options.Lockout.AllowedForNewUsers = true;

	// Configurações de usuário
	options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Configurar autenticação via cookie
builder.Services.ConfigureApplicationCookie(options =>
{
	options.LoginPath = "/Account/Login";
	options.AccessDeniedPath = "/Account/AccessDenied";
});

// Adicione os serviços necessários para Razor Pages
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
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

// Adicionar autenticação e autorização
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
