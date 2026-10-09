using BookyPets.Domain.Common.Interfaces;
using BookyPets.Infrastructure.Common.Persistence;
using BookyPets.Shared.Mediator.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace BookyPets.Infrastructure.Common.Middleware;

public class EventualConsistencyMiddleware(RequestDelegate next, ILogger<EventualConsistencyMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<EventualConsistencyMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context, IMediator publisher, BookyPetsDbContext dbContext)
    {
        var transaction = await dbContext.Database.BeginTransactionAsync();
        var requestMethod = context.Request.Method;
        var requestPath = context.Request.Path.ToString();

        context.Response.OnCompleted(async () =>
        {
            try
            {
                if (context.Items.TryGetValue("DomainEventsQueue", out var value) &&
                    value is Queue<IDomainEvent> domainEventsQueue)
                {
                    while (domainEventsQueue.TryDequeue(out var domainEvent))
                    {
                        await publisher.PublishAsync((INotification)domainEvent);
                    }
                }

                await transaction.CommitAsync();
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    exception,
                    "Domain event processing failed for {Method} {Path}; the request's transaction was rolled back.",
                    requestMethod,
                    requestPath);
            }
            finally
            {
                await transaction.DisposeAsync();
            }
        });

        await _next(context);
    }
}
