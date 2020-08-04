﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RunningDinner.Data;

namespace RunningDinner.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20200213221908_RouteForTeam")]
    partial class RouteForTeam
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
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
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

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
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("RunningDinner.Models.DatabaseModels.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContactData")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ContactId")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("ListRecipientId")
                        .HasColumnType("bigint");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("ProfilePicture")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("SendEventNewsletter")
                        .HasColumnType("bit");

                    b.Property<string>("TemporaryProfilePicture")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("RunningDinner.Models.DatabaseModels.DinnerEvent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ContactList")
                        .HasColumnType("int");

                    b.Property<DateTime>("EventDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("EventEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("EventName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EventPictureLink")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastRegisterDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("PartyLocation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PartyLocationName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("RoutesOpen")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("DinnerEvents");
                });

            modelBuilder.Entity("RunningDinner.Models.DatabaseModels.EventParticipation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AddressAdditions")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AddressNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Allergies")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("DinnerSaver")
                        .HasColumnType("bit");

                    b.Property<int?>("EventId")
                        .HasColumnType("int");

                    b.Property<bool>("InvitationMailSent")
                        .HasColumnType("bit");

                    b.Property<bool>("IsWithoutPartner")
                        .HasColumnType("bit");

                    b.Property<string>("PartnerEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PartnerLastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PartnerName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("RegistrationComplete")
                        .HasColumnType("bit");

                    b.Property<string>("SelectedCourse")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SelectedKitchenOption")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.HasIndex("UserId");

                    b.ToTable("EventParticipations");
                });

            modelBuilder.Entity("RunningDinner.Models.DatabaseModels.Invitation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("EventId")
                        .HasColumnType("int");

                    b.Property<bool>("InvitationAccepted")
                        .HasColumnType("bit");

                    b.Property<string>("InvitationEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("SentTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.HasIndex("UserId");

                    b.ToTable("Invitations");
                });

            modelBuilder.Entity("RunningDinner.Models.DatabaseModels.MailLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("ExceptionMessage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MailTo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StatusCode")
                        .HasColumnType("int");

                    b.Property<string>("StatusMessage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Subject")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("MailLogs");
                });

            modelBuilder.Entity("RunningDinner.Models.DatabaseModels.Route", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("EventId")
                        .HasColumnType("int");

                    b.Property<int?>("FirstCourseGuestTeam1Id")
                        .HasColumnType("int");

                    b.Property<int?>("FirstCourseGuestTeam2Id")
                        .HasColumnType("int");

                    b.Property<int?>("FirstCourseHostTeamId")
                        .HasColumnType("int");

                    b.Property<int?>("RouteForTeamId")
                        .HasColumnType("int");

                    b.Property<int?>("SecondCourseGuestTeam1Id")
                        .HasColumnType("int");

                    b.Property<int?>("SecondCourseGuestTeam2Id")
                        .HasColumnType("int");

                    b.Property<int?>("SecondCourseHostTeamId")
                        .HasColumnType("int");

                    b.Property<int?>("ThirdCourseGuestTeam1Id")
                        .HasColumnType("int");

                    b.Property<int?>("ThirdCourseGuestTeam2Id")
                        .HasColumnType("int");

                    b.Property<int?>("ThirdCourseHostTeamId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.HasIndex("FirstCourseGuestTeam1Id");

                    b.HasIndex("FirstCourseGuestTeam2Id");

                    b.HasIndex("FirstCourseHostTeamId");

                    b.HasIndex("RouteForTeamId");

                    b.HasIndex("SecondCourseGuestTeam1Id");

                    b.HasIndex("SecondCourseGuestTeam2Id");

                    b.HasIndex("SecondCourseHostTeamId");

                    b.HasIndex("ThirdCourseGuestTeam1Id");

                    b.HasIndex("ThirdCourseGuestTeam2Id");

                    b.HasIndex("ThirdCourseHostTeamId");

                    b.ToTable("Routes");
                });

            modelBuilder.Entity("RunningDinner.Models.DatabaseModels.Team", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AddressAdditions")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AddressNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AddressStreet")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Allergies")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("DinnerSaver")
                        .HasColumnType("bit");

                    b.Property<int?>("EventId")
                        .HasColumnType("int");

                    b.Property<string>("FullAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Partner1Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Partner2Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SelectedCourse")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.HasIndex("Partner1Id");

                    b.HasIndex("Partner2Id");

                    b.ToTable("Teams");
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
                    b.HasOne("RunningDinner.Models.DatabaseModels.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("RunningDinner.Models.DatabaseModels.ApplicationUser", null)
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

                    b.HasOne("RunningDinner.Models.DatabaseModels.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("RunningDinner.Models.DatabaseModels.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RunningDinner.Models.DatabaseModels.EventParticipation", b =>
                {
                    b.HasOne("RunningDinner.Models.DatabaseModels.DinnerEvent", "Event")
                        .WithMany()
                        .HasForeignKey("EventId");

                    b.HasOne("RunningDinner.Models.DatabaseModels.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("RunningDinner.Models.DatabaseModels.Invitation", b =>
                {
                    b.HasOne("RunningDinner.Models.DatabaseModels.DinnerEvent", "Event")
                        .WithMany()
                        .HasForeignKey("EventId");

                    b.HasOne("RunningDinner.Models.DatabaseModels.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("RunningDinner.Models.DatabaseModels.Route", b =>
                {
                    b.HasOne("RunningDinner.Models.DatabaseModels.DinnerEvent", "Event")
                        .WithMany()
                        .HasForeignKey("EventId");

                    b.HasOne("RunningDinner.Models.DatabaseModels.Team", "FirstCourseGuestTeam1")
                        .WithMany()
                        .HasForeignKey("FirstCourseGuestTeam1Id");

                    b.HasOne("RunningDinner.Models.DatabaseModels.Team", "FirstCourseGuestTeam2")
                        .WithMany()
                        .HasForeignKey("FirstCourseGuestTeam2Id");

                    b.HasOne("RunningDinner.Models.DatabaseModels.Team", "FirstCourseHostTeam")
                        .WithMany()
                        .HasForeignKey("FirstCourseHostTeamId");

                    b.HasOne("RunningDinner.Models.DatabaseModels.Team", "RouteForTeam")
                        .WithMany()
                        .HasForeignKey("RouteForTeamId");

                    b.HasOne("RunningDinner.Models.DatabaseModels.Team", "SecondCourseGuestTeam1")
                        .WithMany()
                        .HasForeignKey("SecondCourseGuestTeam1Id");

                    b.HasOne("RunningDinner.Models.DatabaseModels.Team", "SecondCourseGuestTeam2")
                        .WithMany()
                        .HasForeignKey("SecondCourseGuestTeam2Id");

                    b.HasOne("RunningDinner.Models.DatabaseModels.Team", "SecondCourseHostTeam")
                        .WithMany()
                        .HasForeignKey("SecondCourseHostTeamId");

                    b.HasOne("RunningDinner.Models.DatabaseModels.Team", "ThirdCourseGuestTeam1")
                        .WithMany()
                        .HasForeignKey("ThirdCourseGuestTeam1Id");

                    b.HasOne("RunningDinner.Models.DatabaseModels.Team", "ThirdCourseGuestTeam2")
                        .WithMany()
                        .HasForeignKey("ThirdCourseGuestTeam2Id");

                    b.HasOne("RunningDinner.Models.DatabaseModels.Team", "ThirdCourseHostTeam")
                        .WithMany()
                        .HasForeignKey("ThirdCourseHostTeamId");
                });

            modelBuilder.Entity("RunningDinner.Models.DatabaseModels.Team", b =>
                {
                    b.HasOne("RunningDinner.Models.DatabaseModels.DinnerEvent", "Event")
                        .WithMany()
                        .HasForeignKey("EventId");

                    b.HasOne("RunningDinner.Models.DatabaseModels.ApplicationUser", "Partner1")
                        .WithMany()
                        .HasForeignKey("Partner1Id");

                    b.HasOne("RunningDinner.Models.DatabaseModels.ApplicationUser", "Partner2")
                        .WithMany()
                        .HasForeignKey("Partner2Id");
                });
#pragma warning restore 612, 618
        }
    }
}
