using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TutorMatch.Data;

namespace TutorMatch.Controllers
	{
	public class DatabaseTestController : Controller
		{
		private readonly ApplicationDbContext _context;

		public DatabaseTestController(ApplicationDbContext context)
			{
			_context = context;
			}

		public async Task<IActionResult> Index()
			{
			try
				{
				// Tenta acessar o banco de dados
				await _context.Database.ExecuteSqlRawAsync("SELECT 1");
				return Content("Conexão com o banco de dados bem-sucedida!");
				}
			catch (Exception ex)
				{
				return Content($"Erro ao conectar ao banco de dados: {ex.Message}");
				}
			}
		}
	}
