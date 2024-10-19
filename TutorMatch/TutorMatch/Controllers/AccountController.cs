using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TutorMatch.Models;

namespace TutorMatch.Controllers
	{
	public class AccountController : Controller
		{
		private readonly UserManager<User> _userManager;
		private readonly SignInManager<User> _signInManager;

		public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
			{
			_userManager = userManager;
			_signInManager = signInManager;
			}

		[HttpGet]
		public IActionResult Register()
			{
			return View();
			}

		[HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel model)
			{
			if (ModelState.IsValid)
				{
				var user = new User
					{
					Name = model.Name,
					Email = model.Email,
					UserName = model.Email,  // o UserName será o email
					UserType = model.UserType
					};

				var result = await _userManager.CreateAsync(user, model.Password);

				if (result.Succeeded)
					{
					// Se o registro foi bem-sucedido, faça o login do usuário automaticamente
					await _signInManager.SignInAsync(user, isPersistent: false);
					return RedirectToAction("Index", "Home"); // Redirecionar após registro bem-sucedido
					}

				// Se houver erros, exibir as mensagens
				foreach (var error in result.Errors)
					{
					ModelState.AddModelError(string.Empty, error.Description);
					}
				}

			return View(model); // Se a validação falhar, mostrar a View com os erros
			}
		}
	}
