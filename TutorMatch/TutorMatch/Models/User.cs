using Microsoft.AspNetCore.Identity;

namespace TutorMatch.Models
	{
	public class User : IdentityUser
		{
		// Propriedade correspondente ao campo "Nome" no formulário de cadastro
		public required string Name { get; set; }

		// Propriedade correspondente ao campo "Tipo de Usuário"
		public required string UserType { get; set; }

		// Você pode adicionar outras propriedades conforme necessário
		}
	}
