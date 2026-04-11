using BookyPets.Application.Authentication.Commands;
using BookyPets.Application.Authentication.Common;
using BookyPets.Application.Authentication.Queries;
using BookyPets.Contracts.Authentication;
using BookyPets.Shared.Mediator.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookyPets.Api.Controllers;

[Route("[controller]")]
[AllowAnonymous]
public class AuthenticationController(IMediator _mediator) : ApiController
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var command = new RegisterCommand(
            request.FirstName,
            request.LastName,
            request.Email,
            request.Password);

        var authResult = await _mediator.SendAsync(command);

        return authResult.Match(
            authResult => Ok(MapToAuthResponse(authResult)),
            Problem);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var query = new LoginQuery(request.Email, request.Password);

        var authResult = await _mediator.SendAsync(query);

        if (!authResult.IsSuccess)
        {
            return Problem(detail: authResult.Error.Description, statusCode: StatusCodes.Status401Unauthorized);
        }

        return authResult.Match(
            authResult => Ok(MapToAuthResponse(authResult)),
            Problem);
    }

    private static AuthenticationResponse MapToAuthResponse(AuthenticationResult authResult)
    {
        return new AuthenticationResponse(
            authResult.Reader.Id,
            authResult.Reader.FirstName,
            authResult.Reader.LastName,
            authResult.Reader.Email,
            authResult.Token
        );
    }
}
