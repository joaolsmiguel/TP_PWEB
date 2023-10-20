using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TP_PWEB2.Data.Migrations
{
    public partial class _2mig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Empresa_ID",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "Empresa",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "dono",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    CategoriaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    AlojamentoId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.CategoriaId);
                });

            migrationBuilder.CreateTable(
                name: "Check_List",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    texto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Confirmado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Check_List", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Alojamento",
                columns: table => new
                {
                    AlojamentoId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    titulo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    descricao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    preco = table.Column<double>(type: "float", nullable: false),
                    categoria_Id = table.Column<int>(type: "int", nullable: true),
                    CategoriaId = table.Column<int>(type: "int", nullable: true),
                    DonoId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    pode_ser_alugado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alojamento", x => x.AlojamentoId);
                    table.ForeignKey(
                        name: "FK_Alojamento_AspNetUsers_DonoId",
                        column: x => x.DonoId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Alojamento_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "CategoriaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CategoryCheckList",
                columns: table => new
                {
                    CategoriaId = table.Column<int>(type: "int", nullable: false),
                    Check_ListId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryCheckList", x => new { x.CategoriaId, x.Check_ListId });
                    table.ForeignKey(
                        name: "FK_CategoryCheckList_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "CategoriaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryCheckList_Check_List_Check_ListId",
                        column: x => x.Check_ListId,
                        principalTable: "Check_List",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reserva",
                columns: table => new
                {
                    ReservaId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    avaliacaoId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AlojamentoId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    check_in = table.Column<DateTime>(type: "datetime2", nullable: false),
                    check_out = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reserva_Confirmada = table.Column<bool>(type: "bit", nullable: false),
                    Entregue = table.Column<bool>(type: "bit", nullable: false),
                    Recebida = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reserva", x => x.ReservaId);
                    table.ForeignKey(
                        name: "FK_Reserva_Alojamento_AlojamentoId",
                        column: x => x.AlojamentoId,
                        principalTable: "Alojamento",
                        principalColumn: "AlojamentoId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reserva_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Avaliacao",
                columns: table => new
                {
                    AvaliacaoId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    avaliacao_do_cliente = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    avalicacao_do_gestor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    id_cliente = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    id_gestor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AlojamentoId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Reserva = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    reservaId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Avaliacao", x => x.AvaliacaoId);
                    table.ForeignKey(
                        name: "FK_Avaliacao_Alojamento_AlojamentoId",
                        column: x => x.AlojamentoId,
                        principalTable: "Alojamento",
                        principalColumn: "AlojamentoId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Avaliacao_Reserva_Reserva",
                        column: x => x.Reserva,
                        principalTable: "Reserva",
                        principalColumn: "ReservaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Imagens_Alojamento",
                columns: table => new
                {
                    imgId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    path = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AlojamentoId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ReservaId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Imagens_Alojamento", x => x.imgId);
                    table.ForeignKey(
                        name: "FK_Imagens_Alojamento_Alojamento_AlojamentoId",
                        column: x => x.AlojamentoId,
                        principalTable: "Alojamento",
                        principalColumn: "AlojamentoId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Imagens_Alojamento_Reserva_ReservaId",
                        column: x => x.ReservaId,
                        principalTable: "Reserva",
                        principalColumn: "ReservaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Categorias",
                columns: new[] { "CategoriaId", "AlojamentoId", "Ativo", "nome" },
                values: new object[] { 1, null, false, "quarto" });

            migrationBuilder.CreateIndex(
                name: "IX_Alojamento_CategoriaId",
                table: "Alojamento",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Alojamento_DonoId",
                table: "Alojamento",
                column: "DonoId");

            migrationBuilder.CreateIndex(
                name: "IX_Avaliacao_AlojamentoId",
                table: "Avaliacao",
                column: "AlojamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Avaliacao_Reserva",
                table: "Avaliacao",
                column: "Reserva",
                unique: true,
                filter: "[Reserva] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryCheckList_Check_ListId",
                table: "CategoryCheckList",
                column: "Check_ListId");

            migrationBuilder.CreateIndex(
                name: "IX_Imagens_Alojamento_AlojamentoId",
                table: "Imagens_Alojamento",
                column: "AlojamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Imagens_Alojamento_ReservaId",
                table: "Imagens_Alojamento",
                column: "ReservaId");

            migrationBuilder.CreateIndex(
                name: "IX_Reserva_AlojamentoId",
                table: "Reserva",
                column: "AlojamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Reserva_UserId",
                table: "Reserva",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Avaliacao");

            migrationBuilder.DropTable(
                name: "CategoryCheckList");

            migrationBuilder.DropTable(
                name: "Imagens_Alojamento");

            migrationBuilder.DropTable(
                name: "Check_List");

            migrationBuilder.DropTable(
                name: "Reserva");

            migrationBuilder.DropTable(
                name: "Alojamento");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropColumn(
                name: "Empresa",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "dono",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "Empresa_ID",
                table: "AspNetUsers",
                type: "int",
                nullable: true);
        }
    }
}
