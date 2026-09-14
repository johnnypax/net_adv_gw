using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace OfflineServiceDeskEF;

public sealed class ServiceDeskDbContext(
    DbContextOptions<ServiceDeskDbContext> options) : DbContext(options)
{

    public DbSet<TicketEntity> Tickets => Set<TicketEntity>();
    public DbSet<NoteEntity> Notes => Set<NoteEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TicketEntity>(ticket =>
        {
            ticket.ToTable("tickets");
            ticket.HasKey(ticket => ticket.Id);

            ticket.Property(ticket => ticket.Title)
                .HasColumnName("title")
                .HasMaxLength(200)
                .IsRequired();

            ticket.Property(item => item.Priority)
                .HasColumnName("priority")
                .HasMaxLength(20)
                .IsRequired();

            ticket.Property(item => item.Status)
               .HasColumnName("status")
               .HasMaxLength(20)
               .IsRequired();

            ticket.Property(item => item.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            //ticket.Property(item => item.Operator)
            //    .HasColumnName("operator")
            //    .HasDefaultValue("N.D.")
            //    .IsRequired();

            ticket.HasIndex(ticket => new
            {
                ticket.Status,
                ticket.Priority
            });

            ticket.HasMany(ticket => ticket.Notes)
                  .WithOne(note => note.Ticket)
                  .HasForeignKey(note => note.TicketId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<NoteEntity>(note =>
        {
            note.ToTable("notes");
            note.HasKey(note => note.Id);
            note.Property(note => note.Text)
                .HasColumnName("text")
                .HasMaxLength(1000)
                .IsRequired();
            note.Property(note => note.CreatedAt)
                .HasColumnName("created_at");

        });
    }
}

public sealed class ServiceDeskDesignTimeFactory 
    : IDesignTimeDbContextFactory<ServiceDeskDbContext>
{
    public ServiceDeskDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<ServiceDeskDbContext>()
            .UseSqlite("Data source=serviceDesk.db").Options;

        return new ServiceDeskDbContext(options);
    }
}
