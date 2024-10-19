using Microsoft.AspNetCore.Mvc;
using TutorMatch.Models; // Certifique-se de que este namespace está correto

public class AccountController : Controller
    {
    // Método GET para a página de registro
    public IActionResult Register()
        {
        return View();
        }

    // Método POST para processar o registro
    [HttpPost]
    public IActionResult Register(RegisterViewModel model)
        {
        if (ModelState.IsValid)
            {
            // Aqui você adicionará a lógica para salvar o usuário no banco de dados.
            // Exemplo:
            // var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            // var result = await _userManager.CreateAsync(user, model.Password);
            // if (result.Succeeded)
            // {
            //     // Código para login ou redirecionamento
            // }

            return RedirectToAction("Index", "Home"); // Redireciona para a página inicial após o registro
            }
        return View(model); // Retorna o modelo com os erros de validação, se houver
        }
    }
