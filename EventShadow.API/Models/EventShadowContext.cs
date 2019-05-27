using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EventShadow.API.Models
{
    public partial class EventShadowContext : DbContext
    {
        public EventShadowContext()
        {
        }

        public EventShadowContext(DbContextOptions<EventShadowContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Devices> Devices { get; set; }
        public virtual DbSet<EventDevices> EventDevices { get; set; }
        public virtual DbSet<Events> Events { get; set; }
        public virtual DbSet<EventShadowDevices> EventShadowDevices { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=(local);Database=EventShadow;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Devices>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AdvertisementType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.BluetoothAddress)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LocalName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ManufacturerDataString)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Rssi)
                    .HasColumnName("RSSI")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.SonarDeviceId).HasColumnName("SonarDeviceID");

                entity.Property(e => e.Timestamp).HasColumnType("datetime");

                entity.HasOne(d => d.SonarDevice)
                    .WithMany(p => p.Devices)
                    .HasForeignKey(d => d.SonarDeviceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Devices_EventShadowDevices");
            });

            modelBuilder.Entity<EventDevices>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.EventId).HasColumnName("EventID");

                entity.Property(e => e.EventShadowId).HasColumnName("EventShadowID");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.EventDevices)
                    .HasForeignKey(d => d.EventId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EventDevices_Events");

                entity.HasOne(d => d.EventShadow)
                    .WithMany(p => p.EventDevices)
                    .HasForeignKey(d => d.EventShadowId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EventDevices_EventShadowDevices");
            });

            modelBuilder.Entity<Events>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Address1).HasColumnType("text");

                entity.Property(e => e.Address2).HasColumnType("text");

                entity.Property(e => e.City).HasColumnType("text");

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.EventName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Market)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.Property(e => e.State)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.Zip)
                    .HasMaxLength(15)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EventShadowDevices>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DeviceName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });
        }
    }
}
