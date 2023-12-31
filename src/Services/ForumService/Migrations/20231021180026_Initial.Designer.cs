﻿// <auto-generated />
using System;
using ForumService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ForumService.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20231021180026_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ForumService.Models.Comment", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreationTimeStamp")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long?>("ParentCommentId")
                        .HasColumnType("bigint");

                    b.Property<long?>("PostId")
                        .IsRequired()
                        .HasColumnType("bigint");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ParentCommentId");

                    b.HasIndex("PostId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("ForumService.Models.Post", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreationTimeStamp")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Header")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long?>("ThemeId")
                        .IsRequired()
                        .HasColumnType("bigint");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ThemeId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("ForumService.Models.Theme", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreationTimeStamp")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long?>("ParentThemeId")
                        .HasColumnType("bigint");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ParentThemeId");

                    b.ToTable("Themes");
                });

            modelBuilder.Entity("ForumService.Models.Comment", b =>
                {
                    b.HasOne("ForumService.Models.Comment", "ParentComment")
                        .WithMany("Comments")
                        .HasForeignKey("ParentCommentId");

                    b.HasOne("ForumService.Models.Post", "Post")
                        .WithMany("Comments")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ParentComment");

                    b.Navigation("Post");
                });

            modelBuilder.Entity("ForumService.Models.Post", b =>
                {
                    b.HasOne("ForumService.Models.Theme", "Theme")
                        .WithMany("Posts")
                        .HasForeignKey("ThemeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Theme");
                });

            modelBuilder.Entity("ForumService.Models.Theme", b =>
                {
                    b.HasOne("ForumService.Models.Theme", "ParentTheme")
                        .WithMany("ChildThemes")
                        .HasForeignKey("ParentThemeId");

                    b.Navigation("ParentTheme");
                });

            modelBuilder.Entity("ForumService.Models.Comment", b =>
                {
                    b.Navigation("Comments");
                });

            modelBuilder.Entity("ForumService.Models.Post", b =>
                {
                    b.Navigation("Comments");
                });

            modelBuilder.Entity("ForumService.Models.Theme", b =>
                {
                    b.Navigation("ChildThemes");

                    b.Navigation("Posts");
                });
#pragma warning restore 612, 618
        }
    }
}
