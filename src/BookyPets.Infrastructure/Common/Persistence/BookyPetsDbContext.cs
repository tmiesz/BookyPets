using System.Reflection;
using BookyPets.Application.Common.Interfaces;
using BookyPets.Domain.BookAggregate;
using BookyPets.Domain.Common;
using BookyPets.Domain.Common.Interfaces;
using BookyPets.Domain.PetAggregate;
using BookyPets.Domain.ReaderAggregate;
using BookyPets.Domain.SessionAggregate;
using BookyPets.Shared.Mediator.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BookyPets.Infrastructure.Common.Persistence;

public class BookyPetsDbContext(DbContextOptions options, IHttpContextAccessor httpContextAccessor, IMediator _mediator) : DbContext(options), IUnitOfWork
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public DbSet<Reader> Readers { get; set; } = null!;
    public DbSet<Pet> Pets { get; set; } = null!;
    public DbSet<Book> Books { get; set; } = null!;
    public DbSet<Progress> Progresses { get; set; } = null!;
    public DbSet<Session> Sessions { get; set; } = null!;

    public async Task CommitChangesAsync()
    {
        //get all the domain events
        var domainEvents = ChangeTracker.Entries<AggregateRoot>()
            .Select(entry => entry.Entity.PopDomainEvents())
            .SelectMany(x => x)
            .ToList();

        //store in http context for later if user is waiting
        if (IsUserWaitingOnline())
        {
            AddDomainEventsToOfflineProcessingQueue(domainEvents);
        }
        else
        {
            await PublishDomainEvents(_mediator, domainEvents);
        }

        await base.SaveChangesAsync();
    }

    public static async Task PublishDomainEvents(IMediator _mediator, List<IDomainEvent> domainEvents)
    {
        foreach (var domainEvent in domainEvents)
        {
            await _mediator.PublishAsync((INotification)domainEvent);
        }
    }

    private bool IsUserWaitingOnline() => _httpContextAccessor.HttpContext is not null;

    private void AddDomainEventsToOfflineProcessingQueue(List<IDomainEvent> domainEvents)
    {
        //fetch q from http context or create new
        var domainEventsQueue = _httpContextAccessor.HttpContext!.Items
            .TryGetValue("DomainEventsQueue", out var value) && value is Queue<IDomainEvent> existingDomainEvents
                ? existingDomainEvents
                : new Queue<IDomainEvent>();

        //add domain events to q
        domainEvents.ForEach(domainEventsQueue.Enqueue);

        //store q in http context
        _httpContextAccessor.HttpContext!.Items["DomainEventsQueue"] = domainEventsQueue;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
