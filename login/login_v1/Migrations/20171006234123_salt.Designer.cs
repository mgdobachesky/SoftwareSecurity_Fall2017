using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using login_v1.Models;

namespace login_v1.Migrations
{
    [DbContext(typeof(login_v1Context))]
    [Migration("20171006234123_salt")]
    partial class salt
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("login_v1.Models.User", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("password");

                    b.Property<string>("salt");

                    b.Property<string>("username");

                    b.HasKey("ID");

                    b.ToTable("User");
                });
        }
    }
}
