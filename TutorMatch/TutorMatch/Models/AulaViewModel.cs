using System.ComponentModel.DataAnnotations;

namespace TutorMatch.Models
	{
	public class AulaViewModel
		{
		public int Id { get; set; }

		[Required(ErrorMessage = "A data da aula é obrigatória.")]
		[DataType(DataType.Date)]
		[Display(Name = "Data da Aula")]
		public required DateTime Data { get; set; }

		[Required(ErrorMessage = "A hora da aula é obrigatória.")]
		[DataType(DataType.Time)]
		[Display(Name = "Hora da Aula")]
		public required TimeSpan Hora { get; set; }

		[Required(ErrorMessage = "O nome da aula é obrigatório.")]
		[Display(Name = "Nome da Aula")]
		public required string NomeDaAula { get; set; }

		[Required(ErrorMessage = "O professor é obrigatório.")]
		public required string ProfessorId { get; set; }

		[Required(ErrorMessage = "O link da aula é obrigatório.")]
		[Url(ErrorMessage = "Formato de URL inválido.")]
		[Display(Name = "Link da Aula")]
		public required string LinkDaAula { get; set; }

		public required virtual User Professor { get; set; }
		}
	}