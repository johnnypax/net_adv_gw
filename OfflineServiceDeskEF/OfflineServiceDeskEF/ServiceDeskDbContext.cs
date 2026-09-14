using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace OfflineServiceDeskEF;

public sealed class ServiceDeskDbContext(
    DbContextOptions<ServiceDeskDbContext> options) : DbContext(options)
{

    public DbSet<TicketEntity> Tickets => Set<TicketEntity>();
    public DbSet<NoteEntity> Notes => Set<NoteEntity>();



}

