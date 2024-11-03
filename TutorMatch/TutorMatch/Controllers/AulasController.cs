using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TutorMatch.Data;
using TutorMatch.Models;

namespace TutorMatch.Controllers
	{
	[Authorize]
	public class AulasController : Controller
		{
		private readonly ApplicationDbContext _context;

		public AulasController(ApplicationDbContext context)
			{
			_context = context;
			}

		// GET: Aulas
		public async Task<IActionResult> Index()
			{
			var applicationDbContext = _context.Aulas.Include(a => a.Professor);
			return View(await applicationDbContext.ToListAsync());
			}

		// GET: Aulas/Details/5
		public async Task<IActionResult> Details(int? id)
			{
			if (id == null)
				{
				return NotFound();
				}

			var aula = await _context.Aulas
				.Include(a => a.Professor)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (aula == null)
				{
				return NotFound();
				}

			return View(aula);
			}

		// GET: Aulas/Create
		public IActionResult Create()
			{
			var professorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var usuario = _context.Users.Find(professorId);
			
			ViewBag.ProfessorName = usuario?.Name;
			ViewBag.ProfessorId = professorId;

			if (User.FindFirst("UserType")?.Value != "Professor")
				{
				TempData["ErrorMessage"] = "Acesso negado. Apenas professores podem criar aulas.";
				return RedirectToAction("Index");
				}

			return View();
			}

		// POST: Aulas/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Id,Data,Hora,NomeDaAula,LinkDaAula")] Aula aula)
			{
			var professorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			// Validação adicional para garantir que o professor está autenticado
			if (string.IsNullOrEmpty(professorId))
				{
				ModelState.AddModelError(string.Empty, "Erro: Usuário não autenticado.");
				return View(aula);
				}

			if (!ModelState.IsValid)
				{
				return View(aula);
				}

			aula.ProfessorId = professorId;

			_context.Add(aula);
			await _context.SaveChangesAsync();

			TempData["SuccessMessage"] = "Aula criada com sucesso!";
			return RedirectToAction(nameof(Index));
			}

		// GET: Aulas/Edit/5
		public async Task<IActionResult> Edit(int? id)
			{
			if (id == null)
				{
				return NotFound();
				}

			var aula = await _context.Aulas.FindAsync(id);
			if (aula == null)
				{
				return NotFound();
				}

			ViewData["ProfessorId"] = new SelectList(_context.Users, "Id", "Name", aula.ProfessorId);
			return View(aula);
			}

		// POST: Aulas/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,Data,Hora,NomeDaAula,ProfessorId,LinkDaAula")] Aula aula)
			{
			if (id != aula.Id)
				{
				return NotFound();
				}

			if (!ModelState.IsValid)
				{
				ViewData["ProfessorId"] = new SelectList(_context.Users, "Id", "Name", aula.ProfessorId);
				return View(aula);
				}

			try
				{
				_context.Update(aula);
				await _context.SaveChangesAsync();

				TempData["SuccessMessage"] = "Aula editada com sucesso!";
				}
			catch (DbUpdateConcurrencyException)
				{
				if (!AulaExists(aula.Id))
					{
					return NotFound();
					}
				else
					{
					throw;
					}
				}

			return RedirectToAction(nameof(Index));
			}

		// GET: Aulas/Delete/5
		public async Task<IActionResult> Delete(int? id)
			{
			if (id == null)
				{
				return NotFound();
				}

			var aula = await _context.Aulas
				.Include(a => a.Professor)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (aula == null)
				{
				return NotFound();
				}

			return View(aula);
			}

		// POST: Aulas/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
			{
			var aula = await _context.Aulas.FindAsync(id);
			if (aula != null)
				{
				_context.Aulas.Remove(aula);
				await _context.SaveChangesAsync();

				TempData["SuccessMessage"] = "Aula excluída com sucesso!";
				}

			return RedirectToAction(nameof(Index));
			}

		private bool AulaExists(int id)
			{
			return _context.Aulas.Any(e => e.Id == id);
			}
		}
	}