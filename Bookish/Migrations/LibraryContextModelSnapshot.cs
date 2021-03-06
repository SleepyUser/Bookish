// <auto-generated />
using System;
using Bookish.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Bookish.Migrations
{
    [DbContext(typeof(LibraryContext))]
    partial class LibraryContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Bookish.Models.Author", b =>
                {
                    b.Property<int>("AuthorID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AuthorID"), 1L, 1);

                    b.Property<string>("AuthorForename")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AuthorSurname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AuthorID");

                    b.ToTable("Authors", (string)null);
                });

            modelBuilder.Entity("Bookish.Models.Book", b =>
                {
                    b.Property<int>("BookID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BookID"), 1L, 1);

                    b.Property<int>("AuthorID")
                        .HasColumnType("int");

                    b.Property<DateTime>("DatePublished")
                        .HasColumnType("datetime2");

                    b.Property<string>("ISBN")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Publisher")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("BookID");

                    b.HasIndex("AuthorID");

                    b.ToTable("Books", (string)null);
                });

            modelBuilder.Entity("Bookish.Models.Borrower", b =>
                {
                    b.Property<int>("BorrowerID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BorrowerID"), 1L, 1);

                    b.Property<string>("Forename")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("BorrowerID");

                    b.ToTable("Borrowers", (string)null);
                });

            modelBuilder.Entity("Bookish.Models.BorrowInstance", b =>
                {
                    b.Property<int>("BorrowInstanceID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BorrowInstanceID"), 1L, 1);

                    b.Property<DateTime>("BorrowDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("BorrowerID")
                        .HasColumnType("int");

                    b.Property<int>("CopyID")
                        .HasColumnType("int");

                    b.Property<bool>("IsOverdue")
                        .HasColumnType("bit");

                    b.Property<DateTime>("ReturnDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Returned")
                        .HasColumnType("bit");

                    b.HasKey("BorrowInstanceID");

                    b.HasIndex("BorrowerID");

                    b.HasIndex("CopyID");

                    b.ToTable("BorrowInstances", (string)null);
                });

            modelBuilder.Entity("Bookish.Models.Copy", b =>
                {
                    b.Property<int>("CopyID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CopyID"), 1L, 1);

                    b.Property<int>("BookID")
                        .HasColumnType("int");

                    b.Property<int?>("BorrowerID")
                        .HasColumnType("int");

                    b.Property<string>("Comments")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CopyID");

                    b.HasIndex("BookID");

                    b.HasIndex("BorrowerID");

                    b.ToTable("Copies", (string)null);
                });

            modelBuilder.Entity("Bookish.Models.Book", b =>
                {
                    b.HasOne("Bookish.Models.Author", null)
                        .WithMany("AuthoredList")
                        .HasForeignKey("AuthorID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Bookish.Models.BorrowInstance", b =>
                {
                    b.HasOne("Bookish.Models.Borrower", null)
                        .WithMany("BorrowList")
                        .HasForeignKey("BorrowerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Bookish.Models.Copy", null)
                        .WithMany("BorrowInstanceList")
                        .HasForeignKey("CopyID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Bookish.Models.Copy", b =>
                {
                    b.HasOne("Bookish.Models.Book", null)
                        .WithMany("CopyList")
                        .HasForeignKey("BookID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Bookish.Models.Borrower", null)
                        .WithMany("CopyList")
                        .HasForeignKey("BorrowerID");
                });

            modelBuilder.Entity("Bookish.Models.Author", b =>
                {
                    b.Navigation("AuthoredList");
                });

            modelBuilder.Entity("Bookish.Models.Book", b =>
                {
                    b.Navigation("CopyList");
                });

            modelBuilder.Entity("Bookish.Models.Borrower", b =>
                {
                    b.Navigation("BorrowList");

                    b.Navigation("CopyList");
                });

            modelBuilder.Entity("Bookish.Models.Copy", b =>
                {
                    b.Navigation("BorrowInstanceList");
                });
#pragma warning restore 612, 618
        }
    }
}
