namespace TutorMatch.Models
	{
	public class LoginViewModel
		{
		// Campo para o email do usuário
		public required string Email { get; set; }

		// Campo para a senha do usuário
		public required string Password { get; set; }

		// Campo para lembrar do login
		public bool RememberMe { get; set; }
		}
	}
