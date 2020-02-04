﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SelecTunes.Backend.Data;

namespace SelecTunes.Backend.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    partial class ApplicationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("SelecTunes.Backend.Models.Party", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("JoinCode")
                        .HasColumnType("text");

                    b.Property<Guid?>("PartyHostId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("PartyHostId");

                    b.ToTable("Parties");
                });

            modelBuilder.Entity("SelecTunes.Backend.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsBanned")
                        .HasColumnType("boolean");

                    b.Property<int?>("PartyId")
                        .HasColumnType("integer");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UserName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("PartyId");

                    b.ToTable("BannedUsers");

                    b.HasDiscriminator<string>("Discriminator").HasValue("User");
                });

            modelBuilder.Entity("SelecTunes.Backend.Models.HostUser", b =>
                {
                    b.HasBaseType("SelecTunes.Backend.Models.User");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SpotifyAccessToken")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SpotifyRefreshToken")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasDiscriminator().HasValue("HostUser");
                });

            modelBuilder.Entity("SelecTunes.Backend.Models.Party", b =>
                {
                    b.HasOne("SelecTunes.Backend.Models.HostUser", "PartyHost")
                        .WithMany()
                        .HasForeignKey("PartyHostId");
                });

            modelBuilder.Entity("SelecTunes.Backend.Models.User", b =>
                {
                    b.HasOne("SelecTunes.Backend.Models.Party", null)
                        .WithMany("BannedMembers")
                        .HasForeignKey("PartyId");
                });
#pragma warning restore 612, 618
        }
    }
}
