using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using MeetingSchema.Model.Entities;
using MeetingSchema.Model;
using MeetingSchema.Data;


namespace MeetingSchema.Data
{
    public class MeetingSchemaContext : DbContext
    {
        public MeetingSchemaContext()
        {

        }
        public DbSet<MeetingSchemas> MeetingSchemas { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Participants> Participants { get; set; }

        public MeetingSchemaContext(DbContextOptions<MeetingSchemaContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }


          //modelBuilder.Entity<MeetingSchemas>().ToTable("MeetingSchemas");

            modelBuilder.Entity<MeetingSchemas>()
                .Property(s => s.CreatorId)
                .IsRequired();

            modelBuilder.Entity<MeetingSchemas>()
                .Property(s => s.DateCreated)
                .HasDefaultValue(DateTime.Now);

            modelBuilder.Entity<MeetingSchemas>()
                .Property(s => s.DateUpdated)
                .HasDefaultValue(DateTime.Now);

            modelBuilder.Entity<MeetingSchemas>()
                .Property(s => s.Type)
                .HasDefaultValue(MeetingSchemaType.Work);

            modelBuilder.Entity<MeetingSchemas>()
                .Property(s => s.Status)
                .HasDefaultValue(MeetingSchemaStatus.Valid);

            modelBuilder.Entity<MeetingSchemas>()
                .HasOne(s => s.Creator)
                .WithMany(c => c.MeetingSchemaCreated);

          //modelBuilder.Entity<User>().ToTable("User");

            modelBuilder.Entity<User>()
                .Property(u => u.Name)
                .HasMaxLength(60)
                .IsRequired();

            //modelBuilder.Entity<Participants>().ToTable("Participants");

            modelBuilder.Entity<Participants>()
                .HasOne(a => a.User)
                .WithMany(u => u.MeetingSchemaAttended)
                .HasForeignKey(a => a.UserId);

            modelBuilder.Entity<Participants>()
                .HasOne(a => a.MeetingSchemas)
                .WithMany(s => s.Participants)
                .HasForeignKey(a => a.MeetingSchemaId);
        }
        }
    }
