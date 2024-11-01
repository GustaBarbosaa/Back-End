﻿// <auto-generated />
using System;
using Infraestrutura.Repositories.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infraestutura.Migrations
{
    [DbContext(typeof(EficazContext))]
    [Migration("20241101191536_updateFoto")]
    partial class updateFoto
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.10");

            modelBuilder.Entity("Core.Models.Endereco", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Cep")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Complemento")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Logradouro")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("SignUpId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("SignUpId");

                    b.ToTable("Enderecos");
                });

            modelBuilder.Entity("Core.Models.SignUp", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CPF")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Cor")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Foto")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Nacionalidade")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("NomeSocial")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Senha")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Sexo")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Telefone")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("SignUps");
                });

            modelBuilder.Entity("Core.Models.Token", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("Expiration")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsRevoked")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("RevokedAt")
                        .HasColumnType("TEXT");

                    b.Property<int?>("SignUpId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("SignUpId");

                    b.ToTable("Tokens");
                });

            modelBuilder.Entity("Core.Models.Endereco", b =>
                {
                    b.HasOne("Core.Models.SignUp", "SignUp")
                        .WithMany("Enderecos")
                        .HasForeignKey("SignUpId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SignUp");
                });

            modelBuilder.Entity("Core.Models.Token", b =>
                {
                    b.HasOne("Core.Models.SignUp", "SignUp")
                        .WithMany("Tokens")
                        .HasForeignKey("SignUpId");

                    b.Navigation("SignUp");
                });

            modelBuilder.Entity("Core.Models.SignUp", b =>
                {
                    b.Navigation("Enderecos");

                    b.Navigation("Tokens");
                });
#pragma warning restore 612, 618
        }
    }
}
