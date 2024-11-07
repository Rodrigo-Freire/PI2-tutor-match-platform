using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TutorMatch.Models;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Collections.Generic;

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

		// Método GET para exibir a página de cadastro
		[HttpGet]
		public IActionResult Register()
			{
			return View();
			}

		// Método POST para processar o cadastro de novos usuários
		[HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel model)
			{
			if (ModelState.IsValid)
				{
				var user = new User
					{
					Name = model.Name,
					Email = model.Email,
					UserName = model.Email,  // O UserName será o email
					UserType = model.UserType
					};

				var result = await _userManager.CreateAsync(user, model.Password);

				if (result.Succeeded)
					{
					// Se o registro foi bem-sucedido, fazer login do usuário
					await _signInManager.SignInAsync(user, isPersistent: false);
					return RedirectToAction("Index", "Home"); // Redireciona para a página inicial
					}

				// Se houver erros, exibir as mensagens em pt-br
				foreach (var error in result.Errors)
					{
					ModelState.AddModelError(string.Empty, TraduzirErro(error.Description));
					}
				}

			// Se a validação falhar, mostrar a View com os erros
			return View(model);
			}

		// Método GET para exibir a página de login
		[HttpGet]
		public IActionResult Login()
			{
			return View();
			}

		// Método POST para processar o login do usuário
		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel model)
			{
			if (ModelState.IsValid)
				{
				// Verifica se o usuário existe
				var user = await _userManager.FindByEmailAsync(model.Email);
				if (user != null)
					{
					// Verifica a senha
					var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: false);
					if (result.Succeeded)
						{
						// Cria uma lista de claims com o tipo de usuário
						var claims = new List<Claim>
						{
							new Claim(ClaimTypes.Name, user.UserName),
							new Claim("UserType", user.UserType) // Adiciona a claim UserType
                        };

						var claimsIdentity = new ClaimsIdentity(claims, "Login");

						// Realiza o login com as claims personalizadas
						await _signInManager.SignInWithClaimsAsync(user, isPersistent: model.RememberMe, claims);

						return RedirectToAction("Index", "Home"); // Redireciona para a página inicial
						}
					}

				// Usando TempData para armazenar a mensagem de erro
				TempData["ErrorMessage"] = "Parece que houve um problema ao fazer o login. Por favor, verifique seu e-mail e senha e tente novamente.";
				}

			return View(model); // Se a validação falhar, mostrar a View com os erros
			}

		// Método POST para processar o logout do usuário
		[HttpPost]
		[ValidateAntiForgeryToken] // Prevenir ataques CSRF
		public async Task<IActionResult> Logout()
			{
			await _signInManager.SignOutAsync();
			TempData["ErrorMessage"] = "Você foi desconectado com sucesso."; // Mensagem de logout
			return RedirectToAction("Index", "Home");
			}

		// Método para traduzir as mensagens de erro do Identity para pt-br
		private static string TraduzirErro(string descricao)
			{
			switch (descricao)
				{
				case "Passwords must have at least one digit ('0'-'9').":
					return "A senha deve conter pelo menos um número ('0'-'9').";
				case "Passwords must have at least one lowercase ('a'-'z').":
					return "A senha deve conter pelo menos uma letra minúscula ('a'-'z').";
				case "Passwords must have at least one uppercase ('A'-'Z').":
					return "A senha deve conter pelo menos uma letra maiúscula ('A'-'Z').";
				case "Passwords must be at least 6 characters.":
					return "A senha deve ter pelo menos 6 caracteres.";
				case "Email 'example@example.com' is already taken.":
					return "O e-mail informado já está em uso.";
				default:
					return descricao; // Retorna a mensagem original caso não haja tradução
				}
			}
		}
	}