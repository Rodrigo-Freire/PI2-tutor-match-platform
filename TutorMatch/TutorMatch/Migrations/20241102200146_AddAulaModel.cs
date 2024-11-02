using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;


#nullable disable

namespace TutorMatch.Migrations
	{
	/// <inheritdoc />
	public partial class AddAulaModel : Migration
		{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
			{
			migrationBuilder.CreateTable(
				name: "Aulas",
				columns: table => new
					{
					Id = table.Column<int>(nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					Data = table.Column<DateTime>(nullable: false),
					Hora = table.Column<TimeSpan>(nullable: false),
					NomeDaAula = table.Column<string>(nullable: false), // usar string para o PostgreSQL
					ProfessorId = table.Column<string>(nullable: false),
					LinkDaAula = table.Column<string>(nullable: false) // usar string para o PostgreSQL
					},
				constraints: table =>
				{
					table.PrimaryKey("PK_Aulas", x => x.Id);
					table.ForeignKey(
						name: "FK_Aulas_AspNetUsers_ProfessorId",
						column: x => x.ProfessorId,
						principalTable: "AspNetUsers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateIndex(
				name: "IX_Aulas_ProfessorId",
				table: "Aulas",
				column: "ProfessorId");
			}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
			{
			migrationBuilder.DropTable(
				name: "Aulas");
			}
		}
	}