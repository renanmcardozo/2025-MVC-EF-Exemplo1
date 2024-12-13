﻿// <auto-generated />
using System;
using MVC_EF.Exemplo1;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MVC_EF.Exemplo1.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241213005825_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.10");

            modelBuilder.Entity("MVC_EF.Exemplo1.Models.Autor", b =>
                {
                    b.Property<int>("AutorID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateOnly?>("AutorDataNascimento")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValue(new DateOnly(1970, 1, 1));

                    b.Property<string>("AutorEmail")
                        .HasMaxLength(80)
                        .HasColumnType("TEXT");

                    b.Property<string>("AutorNome")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("TEXT");

                    b.HasKey("AutorID");

                    b.HasIndex("AutorNome");

                    b.ToTable("Autores");
                });

            modelBuilder.Entity("MVC_EF.Exemplo1.Models.AutorLivro", b =>
                {
                    b.Property<int>("AutorID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("LivroID")
                        .HasColumnType("INTEGER");

                    b.Property<ushort>("OrdemAutoria")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue((ushort)0);

                    b.HasKey("AutorID", "LivroID");

                    b.HasIndex("LivroID");

                    b.ToTable("AutoresLivro");
                });

            modelBuilder.Entity("MVC_EF.Exemplo1.Models.Editora", b =>
                {
                    b.Property<int>("EditoraID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("EditoraCEP")
                        .HasMaxLength(12)
                        .HasColumnType("TEXT");

                    b.Property<string>("EditoraCidade")
                        .HasMaxLength(60)
                        .HasColumnType("TEXT");

                    b.Property<string>("EditoraComplemento")
                        .HasMaxLength(80)
                        .HasColumnType("TEXT");

                    b.Property<string>("EditoraLogradouro")
                        .HasMaxLength(80)
                        .HasColumnType("TEXT");

                    b.Property<string>("EditoraNome")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("TEXT");

                    b.Property<ushort?>("EditoraNumero")
                        .HasColumnType("INTEGER");

                    b.Property<string>("EditoraPais")
                        .HasMaxLength(40)
                        .HasColumnType("TEXT");

                    b.Property<string>("EditoraTelefone")
                        .HasMaxLength(15)
                        .HasColumnType("TEXT");

                    b.Property<string>("EditoraUF")
                        .HasMaxLength(2)
                        .HasColumnType("TEXT");

                    b.HasKey("EditoraID");

                    b.HasIndex("EditoraNome");

                    b.ToTable("Editoras");
                });

            modelBuilder.Entity("MVC_EF.Exemplo1.Models.Livro", b =>
                {
                    b.Property<int>("LivroID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("EditoraID")
                        .HasColumnType("INTEGER");

                    b.Property<ushort>("LivroAnoPublicacao")
                        .HasColumnType("INTEGER");

                    b.Property<string>("LivroISBN")
                        .IsRequired()
                        .HasMaxLength(13)
                        .HasColumnType("TEXT");

                    b.Property<ushort>("LivroPaginas")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue((ushort)0);

                    b.Property<string>("LivroTitulo")
                        .IsRequired()
                        .HasMaxLength(120)
                        .HasColumnType("TEXT");

                    b.HasKey("LivroID");

                    b.HasIndex("EditoraID");

                    b.ToTable("Livros");
                });

            modelBuilder.Entity("MVC_EF.Exemplo1.Models.OperacaoCompraVenda", b =>
                {
                    b.Property<int>("OperacaoID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("LivroID")
                        .HasColumnType("INTEGER");

                    b.Property<DateOnly>("OperacaoData")
                        .HasColumnType("TEXT");

                    b.Property<short>("OperacaoQuantidade")
                        .HasColumnType("INTEGER");

                    b.HasKey("OperacaoID");

                    b.HasIndex("LivroID");

                    b.HasIndex("OperacaoData");

                    b.ToTable("Operacoes");
                });

            modelBuilder.Entity("MVC_EF.Exemplo1.Models.AutorLivro", b =>
                {
                    b.HasOne("MVC_EF.Exemplo1.Models.Autor", "Autor")
                        .WithMany("LivrosDoAutor")
                        .HasForeignKey("AutorID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MVC_EF.Exemplo1.Models.Livro", "Livro")
                        .WithMany("AutoresDoLivro")
                        .HasForeignKey("LivroID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Autor");

                    b.Navigation("Livro");
                });

            modelBuilder.Entity("MVC_EF.Exemplo1.Models.Livro", b =>
                {
                    b.HasOne("MVC_EF.Exemplo1.Models.Editora", "EditoraDoLivro")
                        .WithMany("LivrosDaEditora")
                        .HasForeignKey("EditoraID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EditoraDoLivro");
                });

            modelBuilder.Entity("MVC_EF.Exemplo1.Models.OperacaoCompraVenda", b =>
                {
                    b.HasOne("MVC_EF.Exemplo1.Models.Livro", "LivroDaOperacao")
                        .WithMany("OperacoesDoLivro")
                        .HasForeignKey("LivroID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("LivroDaOperacao");
                });

            modelBuilder.Entity("MVC_EF.Exemplo1.Models.Autor", b =>
                {
                    b.Navigation("LivrosDoAutor");
                });

            modelBuilder.Entity("MVC_EF.Exemplo1.Models.Editora", b =>
                {
                    b.Navigation("LivrosDaEditora");
                });

            modelBuilder.Entity("MVC_EF.Exemplo1.Models.Livro", b =>
                {
                    b.Navigation("AutoresDoLivro");

                    b.Navigation("OperacoesDoLivro");
                });
#pragma warning restore 612, 618
        }
    }
}
