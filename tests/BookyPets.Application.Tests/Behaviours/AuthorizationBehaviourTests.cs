using BookyPets.Application.Common.Authorization;
using BookyPets.Application.Common.Behaviours;
using BookyPets.Application.Common.Interfaces;
using BookyPets.Application.Common.Models;
using BookyPets.Shared.Mediator.Abstractions;
using BookyPets.Shared.Result;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace BookyPets.Application.Tests.Behaviours;

public class AuthorizationBehaviourTests
{
    private readonly ICurrentReaderProvider _currentReaderProvider = Substitute.For<ICurrentReaderProvider>();
    private readonly HandlerDelegate<Result> _next = Substitute.For<HandlerDelegate<Result>>();

    public AuthorizationBehaviourTests()
    {
        _next.Invoke().Returns(Result.Success);
    }

    private record Unprotected : IRequest<Result>;

    [Authorize(Permissions = "books:acquire")]
    private record NeedsPermission : IRequest<Result>;

    [Authorize(Permissions = "books:acquire,pets:acquire")]
    private record NeedsTwoPermissions : IRequest<Result>;

    [Authorize(Roles = "admin")]
    private record NeedsRole : IRequest<Result>;

    [Authorize(Permissions = "books:acquire")]
    [Authorize(Roles = "admin")]
    private record NeedsPermissionAndRole : IRequest<Result>;

    private void SetReader(string[] permissions, string[] roles) =>
        _currentReaderProvider.GetCurrentReader().Returns(new CurrentReader(Guid.NewGuid(), permissions, roles));

    private Task<Result> Handle<TRequest>(TRequest request) where TRequest : IRequest<Result> =>
        new AuthorizationBehaviour<TRequest, Result>(_currentReaderProvider).HandleAsync(request, _next, default);

    [Fact]
    public async Task HandleAsync_WhenRequestHasNoAuthorizeAttribute_ShouldInvokeNextWithoutLookingUpTheReader()
    {
        var result = await Handle(new Unprotected());

        Assert.True(result.IsSuccess);
        _currentReaderProvider.DidNotReceive().GetCurrentReader();
    }

    [Fact]
    public async Task HandleAsync_WhenCurrentReaderCannotBeResolved_ShouldReturnUnauthorized()
    {
        _currentReaderProvider.GetCurrentReader().Throws(new InvalidOperationException("no user"));

        var result = await Handle(new NeedsPermission());

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorType.Unauthorized, result.Error.Type);
        await _next.DidNotReceive().Invoke();
    }

    [Fact]
    public async Task HandleAsync_WhenPermissionIsMissing_ShouldReturnForbidden()
    {
        SetReader(permissions: ["pets:acquire"], roles: []);

        var result = await Handle(new NeedsPermission());

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorType.Forbidden, result.Error.Type);
        Assert.Equal("ReaderForbidden", result.Error.Code);
        await _next.DidNotReceive().Invoke();
    }

    [Fact]
    public async Task HandleAsync_WhenPermissionIsPresent_ShouldInvokeNext()
    {
        SetReader(permissions: ["books:acquire", "extra:permission"], roles: []);

        var result = await Handle(new NeedsPermission());

        Assert.True(result.IsSuccess);
        await _next.Received(1).Invoke();
    }

    [Fact]
    public async Task HandleAsync_WhenOnlyOneOfTwoRequiredPermissionsIsPresent_ShouldReturnForbidden()
    {
        SetReader(permissions: ["books:acquire"], roles: []);

        var result = await Handle(new NeedsTwoPermissions());

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorType.Forbidden, result.Error.Type);
    }

    [Fact]
    public async Task HandleAsync_WhenBothRequiredPermissionsArePresent_ShouldInvokeNext()
    {
        SetReader(permissions: ["books:acquire", "pets:acquire"], roles: []);

        var result = await Handle(new NeedsTwoPermissions());

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task HandleAsync_WhenRoleIsMissing_ShouldReturnForbidden()
    {
        SetReader(permissions: ["books:acquire"], roles: []);

        var result = await Handle(new NeedsRole());

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorType.Forbidden, result.Error.Type);
        Assert.Equal("ReaderForbidden", result.Error.Code);
        await _next.DidNotReceive().Invoke();
    }

    [Fact]
    public async Task HandleAsync_WhenRoleIsPresent_ShouldInvokeNext()
    {
        SetReader(permissions: [], roles: ["admin"]);

        var result = await Handle(new NeedsRole());

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task HandleAsync_WhenPermissionIsPresentButRoleIsMissing_ShouldReturnForbidden()
    {
        SetReader(permissions: ["books:acquire"], roles: []);

        var result = await Handle(new NeedsPermissionAndRole());

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorType.Forbidden, result.Error.Type);
    }

    [Fact]
    public async Task HandleAsync_WhenRoleIsPresentButPermissionIsMissing_ShouldReturnForbidden()
    {
        SetReader(permissions: [], roles: ["admin"]);

        var result = await Handle(new NeedsPermissionAndRole());

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorType.Forbidden, result.Error.Type);
    }

    [Fact]
    public async Task HandleAsync_WhenPermissionAndRoleArePresent_ShouldInvokeNext()
    {
        SetReader(permissions: ["books:acquire"], roles: ["admin"]);

        var result = await Handle(new NeedsPermissionAndRole());

        Assert.True(result.IsSuccess);
    }
}
