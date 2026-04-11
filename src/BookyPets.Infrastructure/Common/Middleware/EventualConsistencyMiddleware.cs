using BookyPets.Domain.Common.Interfaces;
using BookyPets.Infrastructure.Common.Persistence;
using BookyPets.Shared.Mediator.Abstractions;
using Microsoft.AspNetCore.Http;

namespace BookyPets.Infrastructure.Common.Middleware;

public class EventualConsistencyMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context, IMediator publisher, BookyPetsDbContext dbContext)
    {
        var transaction = await dbContext.Database.BeginTransactionAsync();

        context.Response.OnCompleted(async () =>
        {
            try
            {
                if (context.Items.TryGetValue("DomainEventsQueue", out var value) &&
                    value is Queue<IDomainEvent> domainEventsQueue)
                {
                    while (domainEventsQueue!.TryDequeue(out var domainEvent))
                    {
                        await publisher.PublishAsync((INotification)domainEvent);
                    }
                }

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                //notify the victim that their transaction failed.. or not
            }
            finally
            {
                await transaction.DisposeAsync();
            }
        });

        await _next(context);
    }
}
