using System;
using System.ComponentModel.DataAnnotations;

namespace TutorMatch.Models
	{
	public class Aula
		{
		public int Id { get; set; }

		private DateTime _data;

		[Required(ErrorMessage = "A data da aula é obrigatória.")]
		[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
		public DateTime Data
			{
			get => _data;
			set => _data = DateTime.SpecifyKind(value, DateTimeKind.Utc);
			}

		[Required(ErrorMessage = "A hora da aula é obrigatória.")]
		public required TimeSpan Hora { get; set; }

		[Required(ErrorMessage = "O nome da aula é obrigatório.")]
		public required string NomeDaAula { get; set; }

		[Required(ErrorMessage = "O professor é obrigatório.")]
		public required string ProfessorId { get; set; }

		[Required(ErrorMessage = "O link da aula é obrigatório.")]
		[Url(ErrorMessage = "Formato de URL inválido.")]
		public required string LinkDaAula { get; set; }

		public virtual User? Professor { get; set; } // Navegação para a classe User
		}
	}
