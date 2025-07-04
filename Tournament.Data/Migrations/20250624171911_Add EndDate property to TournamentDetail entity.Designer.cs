﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Tournament.Infrastructure.Data;

#nullable disable

namespace Tournament.Data.Migrations
{
    [DbContext(typeof(TournamentApiContext))]
    [Migration("20250624171911_Add EndDate property to TournamentDetail entity")]
    partial class AddEndDatepropertytoTournamentDetailentity
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Tournament.Core.Entities.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Time")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TournamentDetailId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TournamentDetailId");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("Tournament.Core.Entities.TournamentDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("TournamentDetails");
                });

            modelBuilder.Entity("Tournament.Core.Entities.Game", b =>
                {
                    b.HasOne("Tournament.Core.Entities.TournamentDetail", "TournamentDetail")
                        .WithMany("Games")
                        .HasForeignKey("TournamentDetailId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TournamentDetail");
                });

            modelBuilder.Entity("Tournament.Core.Entities.TournamentDetail", b =>
                {
                    b.Navigation("Games");
                });
#pragma warning restore 612, 618
        }
    }
}
