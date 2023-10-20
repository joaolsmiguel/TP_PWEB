﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TP_PWEB2.Data;

namespace TP_PWEB2.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.13")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("TP_PWEB2.Data.AppUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("Empresa")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("dono")
                        .HasColumnType("bit");

                    b.Property<string>("pNome")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("uNome")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("TP_PWEB2.Models.Alojamento", b =>
                {
                    b.Property<string>("AlojamentoId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("CategoriaId")
                        .HasColumnType("int");

                    b.Property<string>("DonoId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("categoria_Id")
                        .HasColumnType("int");

                    b.Property<string>("descricao")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("pode_ser_alugado")
                        .HasColumnType("bit");

                    b.Property<double>("preco")
                        .HasColumnType("float");

                    b.Property<string>("titulo")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AlojamentoId");

                    b.HasIndex("CategoriaId");

                    b.HasIndex("DonoId");

                    b.ToTable("Alojamento");
                });

            modelBuilder.Entity("TP_PWEB2.Models.Avaliacao", b =>
                {
                    b.Property<string>("AvaliacaoId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AlojamentoId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Reserva")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("avaliacao_do_cliente")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("avalicacao_do_gestor")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("id_cliente")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("id_gestor")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("reservaId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AvaliacaoId");

                    b.HasIndex("AlojamentoId");

                    b.HasIndex("Reserva")
                        .IsUnique()
                        .HasFilter("[Reserva] IS NOT NULL");

                    b.ToTable("Avaliacao");
                });

            modelBuilder.Entity("TP_PWEB2.Models.Categoria", b =>
                {
                    b.Property<int>("CategoriaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AlojamentoId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Ativo")
                        .HasColumnType("bit");

                    b.Property<string>("nome")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CategoriaId");

                    b.ToTable("Categorias");

                    b.HasData(
                        new
                        {
                            CategoriaId = 1,
                            Ativo = false,
                            nome = "quarto"
                        });
                });

            modelBuilder.Entity("TP_PWEB2.Models.CategoriaCheck_List", b =>
                {
                    b.Property<int>("CategoriaId")
                        .HasColumnType("int");

                    b.Property<int>("Check_ListId")
                        .HasColumnType("int");

                    b.HasKey("CategoriaId", "Check_ListId");

                    b.HasIndex("Check_ListId");

                    b.ToTable("CategoryCheckList");
                });

            modelBuilder.Entity("TP_PWEB2.Models.Check_List", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Confirmado")
                        .HasColumnType("bit");

                    b.Property<string>("texto")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("Check_List");
                });

            modelBuilder.Entity("TP_PWEB2.Models.Imagens_Alojamento", b =>
                {
                    b.Property<int>("imgId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AlojamentoId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ReservaId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("path")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("imgId");

                    b.HasIndex("AlojamentoId");

                    b.HasIndex("ReservaId");

                    b.ToTable("Imagens_Alojamento");
                });

            modelBuilder.Entity("TP_PWEB2.Models.Reserva", b =>
                {
                    b.Property<string>("ReservaId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AlojamentoId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("Entregue")
                        .HasColumnType("bit");

                    b.Property<bool>("Recebida")
                        .HasColumnType("bit");

                    b.Property<bool>("Reserva_Confirmada")
                        .HasColumnType("bit");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("avaliacaoId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("check_in")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("check_out")
                        .HasColumnType("datetime2");

                    b.HasKey("ReservaId");

                    b.HasIndex("AlojamentoId");

                    b.HasIndex("UserId");

                    b.ToTable("Reserva");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("TP_PWEB2.Data.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("TP_PWEB2.Data.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TP_PWEB2.Data.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("TP_PWEB2.Data.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TP_PWEB2.Models.Alojamento", b =>
                {
                    b.HasOne("TP_PWEB2.Models.Categoria", "Categoria")
                        .WithMany("alojamentos")
                        .HasForeignKey("CategoriaId");

                    b.HasOne("TP_PWEB2.Data.AppUser", "Dono")
                        .WithMany("Alojamentos")
                        .HasForeignKey("DonoId");

                    b.Navigation("Categoria");

                    b.Navigation("Dono");
                });

            modelBuilder.Entity("TP_PWEB2.Models.Avaliacao", b =>
                {
                    b.HasOne("TP_PWEB2.Models.Alojamento", "alojamento")
                        .WithMany("Avaliacoes")
                        .HasForeignKey("AlojamentoId");

                    b.HasOne("TP_PWEB2.Models.Reserva", "reserva")
                        .WithOne("avaliacao")
                        .HasForeignKey("TP_PWEB2.Models.Avaliacao", "Reserva");

                    b.Navigation("alojamento");

                    b.Navigation("reserva");
                });

            modelBuilder.Entity("TP_PWEB2.Models.CategoriaCheck_List", b =>
                {
                    b.HasOne("TP_PWEB2.Models.Categoria", "Categoria")
                        .WithMany("CategoriaCheck_List")
                        .HasForeignKey("CategoriaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TP_PWEB2.Models.Check_List", "Check")
                        .WithMany("CategoriaCheck_List")
                        .HasForeignKey("Check_ListId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Categoria");

                    b.Navigation("Check");
                });

            modelBuilder.Entity("TP_PWEB2.Models.Imagens_Alojamento", b =>
                {
                    b.HasOne("TP_PWEB2.Models.Alojamento", "alojamento")
                        .WithMany("imagens")
                        .HasForeignKey("AlojamentoId");

                    b.HasOne("TP_PWEB2.Models.Reserva", "Reserva")
                        .WithMany("img_dados")
                        .HasForeignKey("ReservaId");

                    b.Navigation("alojamento");

                    b.Navigation("Reserva");
                });

            modelBuilder.Entity("TP_PWEB2.Models.Reserva", b =>
                {
                    b.HasOne("TP_PWEB2.Models.Alojamento", "Alojamento")
                        .WithMany("Reservas")
                        .HasForeignKey("AlojamentoId");

                    b.HasOne("TP_PWEB2.Data.AppUser", "User")
                        .WithMany("Reservas")
                        .HasForeignKey("UserId");

                    b.Navigation("Alojamento");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TP_PWEB2.Data.AppUser", b =>
                {
                    b.Navigation("Alojamentos");

                    b.Navigation("Reservas");
                });

            modelBuilder.Entity("TP_PWEB2.Models.Alojamento", b =>
                {
                    b.Navigation("Avaliacoes");

                    b.Navigation("imagens");

                    b.Navigation("Reservas");
                });

            modelBuilder.Entity("TP_PWEB2.Models.Categoria", b =>
                {
                    b.Navigation("alojamentos");

                    b.Navigation("CategoriaCheck_List");
                });

            modelBuilder.Entity("TP_PWEB2.Models.Check_List", b =>
                {
                    b.Navigation("CategoriaCheck_List");
                });

            modelBuilder.Entity("TP_PWEB2.Models.Reserva", b =>
                {
                    b.Navigation("avaliacao");

                    b.Navigation("img_dados");
                });
#pragma warning restore 612, 618
        }
    }
}