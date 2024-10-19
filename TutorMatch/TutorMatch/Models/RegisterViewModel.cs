using System.ComponentModel.DataAnnotations;

namespace TutorMatch.Models
	{
	public class RegisterViewModel
		{
		[Required(ErrorMessage = "O nome é obrigatório.")]
		public required string Name { get; set; }

		[Required(ErrorMessage = "O e-mail é obrigatório.")]
		[EmailAddress(ErrorMessage = "Formato de e-mail inválido.")]
		public required string Email { get; set; }

		[Required(ErrorMessage = "A senha é obrigatória.")]
		[DataType(DataType.Password)]
		[StringLength(100, ErrorMessage = "A {0} deve ter pelo menos {2} caracteres.", MinimumLength = 6)]
		public required string Password { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Confirme sua senha")]
		[Compare("Password", ErrorMessage = "As senhas não conferem.")]
		public required string ConfirmPassword { get; set; }

		[Required(ErrorMessage = "O tipo de usuário é obrigatório.")]
		public required string UserType { get; set; }  // Diferenciar entre aluno e professor
		}
	}
