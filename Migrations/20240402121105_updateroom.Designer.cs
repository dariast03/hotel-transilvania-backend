﻿// <auto-generated />
using System;
using HotelTransilvania.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HotelTransilvania.Migrations
{
    [DbContext(typeof(RegisterLoginContext))]
    [Migration("20240402121105_updateroom")]
    partial class updateroom
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.17")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("HotelTransilvania.Models.Cliente", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Frecuencia")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("IdPersona")
                        .HasColumnType("int");

                    b.Property<int>("IdUsuario")
                        .HasColumnType("int");

                    b.Property<string>("Nacionalidad")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("IdPersona");

                    b.HasIndex("IdUsuario");

                    b.ToTable("Cliente");
                });

            modelBuilder.Entity("HotelTransilvania.Models.Habitacion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Capacidad")
                        .HasColumnType("int");

                    b.Property<string>("Codigo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Estado")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Imagen")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("NroBaños")
                        .HasColumnType("int");

                    b.Property<int>("NroCamas")
                        .HasColumnType("int");

                    b.Property<int>("NroCuartos")
                        .HasColumnType("int");

                    b.Property<float>("Precio")
                        .HasColumnType("real");

                    b.Property<string>("Tipo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Ubicacion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Wifi")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("Habitacion");
                });

            modelBuilder.Entity("HotelTransilvania.Models.Persona", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Apellido")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Direccion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NroDocumento")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Telefono")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TipoDocumento")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Persona");
                });

            modelBuilder.Entity("HotelTransilvania.Models.Promocion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<float>("Descuento")
                        .HasColumnType("real");

                    b.Property<string>("Estado")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("IdRecepcionista")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("IdRecepcionista");

                    b.ToTable("Promocion");
                });

            modelBuilder.Entity("HotelTransilvania.Models.Recepcionista", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Cargo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("IdPersona")
                        .HasColumnType("int");

                    b.Property<int>("IdUsuario")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("IdPersona");

                    b.HasIndex("IdUsuario");

                    b.ToTable("Recepcionista");
                });

            modelBuilder.Entity("HotelTransilvania.Models.Reserva", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Estado")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("FechaFin")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FechaInicio")
                        .HasColumnType("datetime2");

                    b.Property<int>("IdCliente")
                        .HasColumnType("int");

                    b.Property<int>("IdHabitacion")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("IdCliente");

                    b.HasIndex("IdHabitacion");

                    b.ToTable("Reserva");
                });

            modelBuilder.Entity("HotelTransilvania.Models.Usuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Contraseña")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Correo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Estado")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Rol")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Usuario");
                });

            modelBuilder.Entity("HotelTransilvania.Models.Cliente", b =>
                {
                    b.HasOne("HotelTransilvania.Models.Persona", "Persona")
                        .WithMany()
                        .HasForeignKey("IdPersona")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HotelTransilvania.Models.Usuario", "Usuario")
                        .WithMany()
                        .HasForeignKey("IdUsuario")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Persona");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("HotelTransilvania.Models.Promocion", b =>
                {
                    b.HasOne("HotelTransilvania.Models.Recepcionista", "Recepcionista")
                        .WithMany()
                        .HasForeignKey("IdRecepcionista")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Recepcionista");
                });

            modelBuilder.Entity("HotelTransilvania.Models.Recepcionista", b =>
                {
                    b.HasOne("HotelTransilvania.Models.Persona", "Persona")
                        .WithMany()
                        .HasForeignKey("IdPersona")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HotelTransilvania.Models.Usuario", "Usuario")
                        .WithMany()
                        .HasForeignKey("IdUsuario")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Persona");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("HotelTransilvania.Models.Reserva", b =>
                {
                    b.HasOne("HotelTransilvania.Models.Cliente", "Cliente")
                        .WithMany()
                        .HasForeignKey("IdCliente")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HotelTransilvania.Models.Habitacion", "Habitacion")
                        .WithMany()
                        .HasForeignKey("IdHabitacion")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cliente");

                    b.Navigation("Habitacion");
                });
#pragma warning restore 612, 618
        }
    }
}