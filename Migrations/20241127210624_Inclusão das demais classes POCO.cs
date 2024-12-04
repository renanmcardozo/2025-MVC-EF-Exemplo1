using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVC_EF.Exemplo1.Migrations
{
    /// <inheritdoc />
    public partial class InclusãodasdemaisclassesPOCO : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EditoraID",
                table: "Livros",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<ushort>(
                name: "LivroAnoPublicacao",
                table: "Livros",
                type: "INTEGER",
                nullable: false,
                defaultValue: (ushort)0);

            migrationBuilder.AddColumn<string>(
                name: "LivroISBN",
                table: "Livros",
                type: "TEXT",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<ushort>(
                name: "LivroPaginas",
                table: "Livros",
                type: "INTEGER",
                nullable: false,
                defaultValue: (ushort)0);

            migrationBuilder.CreateTable(
                name: "Autores",
                columns: table => new
                {
                    AutorID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AutorNome = table.Column<string>(type: "TEXT", maxLength: 80, nullable: false),
                    AutorDataNascimento = table.Column<DateOnly>(type: "TEXT", nullable: true, defaultValue: new DateOnly(1970, 1, 1)),
                    AutorEmail = table.Column<string>(type: "TEXT", maxLength: 80, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Autores", x => x.AutorID);
                });

            migrationBuilder.CreateTable(
                name: "Editoras",
                columns: table => new
                {
                    EditoraID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EditoraNome = table.Column<string>(type: "TEXT", maxLength: 80, nullable: false),
                    EditoraLogradouro = table.Column<string>(type: "TEXT", maxLength: 80, nullable: true),
                    EditoraNumero = table.Column<ushort>(type: "INTEGER", nullable: true),
                    EditoraComplemento = table.Column<string>(type: "TEXT", maxLength: 80, nullable: true),
                    EditoraCidade = table.Column<string>(type: "TEXT", maxLength: 60, nullable: false),
                    EditoraUF = table.Column<string>(type: "TEXT", maxLength: 2, nullable: false),
                    EditoraPais = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    EditoraCEP = table.Column<string>(type: "TEXT", maxLength: 12, nullable: true),
                    EditoraTelefone = table.Column<string>(type: "TEXT", maxLength: 15, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Editoras", x => x.EditoraID);
                });

            migrationBuilder.CreateTable(
                name: "Operacoes",
                columns: table => new
                {
                    OperacaoID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LivroID = table.Column<int>(type: "INTEGER", nullable: false),
                    OperacaoData = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    OperacaoQuantidade = table.Column<short>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operacoes", x => x.OperacaoID);
                    table.ForeignKey(
                        name: "FK_Operacoes_Livros_LivroID",
                        column: x => x.LivroID,
                        principalTable: "Livros",
                        principalColumn: "LivroID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AutoresLivro",
                columns: table => new
                {
                    LivroID = table.Column<int>(type: "INTEGER", nullable: false),
                    AutorID = table.Column<int>(type: "INTEGER", nullable: false),
                    OrdemAutoria = table.Column<ushort>(type: "INTEGER", nullable: false, defaultValue: (ushort)0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutoresLivro", x => new { x.AutorID, x.LivroID });
                    table.ForeignKey(
                        name: "FK_AutoresLivro_Autores_AutorID",
                        column: x => x.AutorID,
                        principalTable: "Autores",
                        principalColumn: "AutorID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AutoresLivro_Livros_LivroID",
                        column: x => x.LivroID,
                        principalTable: "Livros",
                        principalColumn: "LivroID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Livros_EditoraID",
                table: "Livros",
                column: "EditoraID");

            migrationBuilder.CreateIndex(
                name: "IX_Autores_AutorNome",
                table: "Autores",
                column: "AutorNome");

            migrationBuilder.CreateIndex(
                name: "IX_AutoresLivro_LivroID",
                table: "AutoresLivro",
                column: "LivroID");

            migrationBuilder.CreateIndex(
                name: "IX_Editoras_EditoraNome",
                table: "Editoras",
                column: "EditoraNome");

            migrationBuilder.CreateIndex(
                name: "IX_Operacoes_LivroID",
                table: "Operacoes",
                column: "LivroID");

            migrationBuilder.CreateIndex(
                name: "IX_Operacoes_OperacaoData",
                table: "Operacoes",
                column: "OperacaoData");

            migrationBuilder.AddForeignKey(
                name: "FK_Livros_Editoras_EditoraID",
                table: "Livros",
                column: "EditoraID",
                principalTable: "Editoras",
                principalColumn: "EditoraID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Livros_Editoras_EditoraID",
                table: "Livros");

            migrationBuilder.DropTable(
                name: "AutoresLivro");

            migrationBuilder.DropTable(
                name: "Editoras");

            migrationBuilder.DropTable(
                name: "Operacoes");

            migrationBuilder.DropTable(
                name: "Autores");

            migrationBuilder.DropIndex(
                name: "IX_Livros_EditoraID",
                table: "Livros");

            migrationBuilder.DropColumn(
                name: "EditoraID",
                table: "Livros");

            migrationBuilder.DropColumn(
                name: "LivroAnoPublicacao",
                table: "Livros");

            migrationBuilder.DropColumn(
                name: "LivroISBN",
                table: "Livros");

            migrationBuilder.DropColumn(
                name: "LivroPaginas",
                table: "Livros");
        }
    }
}
