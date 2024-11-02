using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TutorMatch.Data;
using TutorMatch.Models;

namespace TutorMatch.Controllers
	{
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
			ViewData["ProfessorId"] = new SelectList(_context.Users, "Id", "Id");
			return View();
			}

		// POST: Aulas/Create
		// Para proteger contra ataques de overposting, habilite as propriedades específicas que deseja vincular.
		// Para mais detalhes, veja http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Id,Data,Hora,NomeDaAula,ProfessorId,LinkDaAula")] Aula aula)
			{
			if (ModelState.IsValid)
				{
				_context.Add(aula);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
				}
			ViewData["ProfessorId"] = new SelectList(_context.Users, "Id", "Id", aula.ProfessorId);
			return View(aula);
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
			ViewData["ProfessorId"] = new SelectList(_context.Users, "Id", "Id", aula.ProfessorId);
			return View(aula);
			}

		// POST: Aulas/Edit/5
		// Para proteger contra ataques de overposting, habilite as propriedades específicas que deseja vincular.
		// Para mais detalhes, veja http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,Data,Hora,NomeDaAula,ProfessorId,LinkDaAula")] Aula aula)
			{
			if (id != aula.Id)
				{
				return NotFound();
				}

			if (ModelState.IsValid)
				{
				try
					{
					_context.Update(aula);
					await _context.SaveChangesAsync();
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
			ViewData["ProfessorId"] = new SelectList(_context.Users, "Id", "Id", aula.ProfessorId);
			return View(aula);
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
				}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
			}

		private bool AulaExists(int id)
			{
			return _context.Aulas.Any(e => e.Id == id);
			}
		}
	}