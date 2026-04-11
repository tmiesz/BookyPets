using BookyPets.Api.Common;
using BookyPets.Application.Common.Authorization;
using BookyPets.Application.Sessions.Commands.AbandonSession;
using BookyPets.Application.Sessions.Commands.CompleteSession;
using BookyPets.Application.Sessions.Commands.StartSession;
using BookyPets.Contracts.Sessions;
using BookyPets.Shared.Mediator.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace BookyPets.Api.Controllers;

[Authorize]
[Route("[controller]")]
public class SessionsController(IMediator _mediator) : ApiController
{
    [HttpPost("start")]
    public async Task<IActionResult> StartSession(StartSessionRequest request)
    {
        var command = new StartSessionCommand(request.ProgressId, request.PetId);

        var startSessionResult = await _mediator.SendAsync(command);

        return startSessionResult.Match(
            session => Ok(new SessionResponse(session.Id, DtoConverter.ToDto(session.Status), session.PagesRead, session.EndTime)),
            Problem);
    }

    [HttpPost("abandon")]
    public async Task<IActionResult> AbandonSession(AbandonSessionRequest request)
    {
        var command = new AbandonSessionCommand(request.SessionId);

        var abandonSessionResult = await _mediator.SendAsync(command);

        return abandonSessionResult.Match(
            session => Ok(new SessionResponse(session.Id, DtoConverter.ToDto(session.Status), session.PagesRead, session.EndTime)),
            Problem);
    }

    [HttpPost("complete")]
    public async Task<IActionResult> CompleteSession(CompleteSessionRequest request)
    {
        var command = new CompleteSessionCommand(request.SessionId, request.PagesRead);

        var completeSessionResult = await _mediator.SendAsync(command);

        return completeSessionResult.Match(
            session => Ok(new SessionResponse(session.Id, DtoConverter.ToDto(session.Status), session.PagesRead, session.EndTime)),
            Problem);
    }
}
