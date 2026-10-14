using System.Net;
using System.Net.Http.Json;
using System.Text;
using BookyPets.Api.Tests.Common;
using BookyPets.Contracts.Sessions;
using Common.Tests.TestConstants;

namespace BookyPets.Api.Tests.Controllers.SessionsController;

[Collection(BookyPetsApiFactoryCollection.CollectionName)]
public class SessionsTests
{
    private readonly BookyPetsApiFactory _apiFactory;

    public SessionsTests(BookyPetsApiFactory apiFactory)
    {
        _apiFactory = apiFactory;
        apiFactory.ResetDatabase();
    }

    private async Task<(HttpClient Reader, Guid ProgressId, SessionResponse Session)> ArrangeActiveSessionAsync()
    {
        var auth = await _apiFactory.CreateAnonymousClient().RegisterAsync();
        var reader = _apiFactory.CreateReaderClient(auth.Id);
        var book = await _apiFactory.HttpClient.CreateBookAsync();
        var progressId = await reader.AcquireBookAsync(book.Id);
        var session = await reader.StartSessionAsync(progressId);

        return (reader, progressId, session);
    }

    private static async Task AssertSessionStillActiveAsync(HttpClient reader, Guid sessionId)
    {
        var response = await reader.GetAsync("Sessions/active");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var active = await response.ReadRequiredAsync<ActiveSessionResponse>();
        Assert.Equal(sessionId, active.Id);
    }

    [Fact]
    public async Task StartSession_WhenReaderAlreadyHasAnActiveSession_ShouldReturnConflict()
    {
        var (reader, progressId, _) = await ArrangeActiveSessionAsync();

        var response = await reader.PostAsJsonAsync("Sessions/start", new StartSessionRequest(progressId, null));

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        Assert.Equal("Session.AlreadyActive", await response.ReadProblemCodeAsync());
    }

    [Fact]
    public async Task CompleteSession_WhenPagesReadIsMissing_ShouldReturnBadRequestAndKeepTheSessionActive()
    {
        var (reader, _, session) = await ArrangeActiveSessionAsync();
        var body = new StringContent($$"""{"sessionId":"{{session.Id}}"}""", Encoding.UTF8, "application/json");

        var response = await reader.PostAsync("Sessions/complete", body);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        await AssertSessionStillActiveAsync(reader, session.Id);
    }

    [Fact]
    public async Task CompleteSession_WhenBodyIsAnEmptyObject_ShouldReturnBadRequest()
    {
        var (reader, _, session) = await ArrangeActiveSessionAsync();
        var body = new StringContent("{}", Encoding.UTF8, "application/json");

        var response = await reader.PostAsync("Sessions/complete", body);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        await AssertSessionStillActiveAsync(reader, session.Id);
    }

    [Fact]
    public async Task CompleteSession_WhenPagesReadIsNegative_ShouldReturnBadRequestWithTheFailingProperty()
    {
        var (reader, _, session) = await ArrangeActiveSessionAsync();

        var response = await reader.PostAsJsonAsync("Sessions/complete", new CompleteSessionRequest(session.Id, -5));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("PagesRead", await response.ReadProblemCodeAsync());
        await AssertSessionStillActiveAsync(reader, session.Id);
    }

    [Fact]
    public async Task CompleteSession_WhenPagesReadIsExplicitlyZero_ShouldCompleteWithoutChangingProgress()
    {
        var (reader, progressId, session) = await ArrangeActiveSessionAsync();

        var completed = await reader.CompleteSessionAsync(session.Id, pagesRead: 0);

        Assert.Equal(SessionStatus.Completed, completed.Status);
        Assert.Equal(0, completed.PagesRead);
        Assert.NotNull(completed.EndTime);

        var progress = await reader.WaitForProgressAsync(progressId, expectedCurrentPage: 0);
        Assert.Equal(Constants.Book.PageCount, progress.TotalPages);
    }

    [Fact]
    public async Task CompleteSession_WhenSessionDoesNotExist_ShouldReturnNotFound()
    {
        var (reader, _, _) = await ArrangeActiveSessionAsync();

        var response = await reader.PostAsJsonAsync("Sessions/complete", new CompleteSessionRequest(Guid.NewGuid(), 10));

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("SessionNotFound", await response.ReadProblemCodeAsync());
    }

    [Fact]
    public async Task CompleteSession_WhenSessionIsAlreadyCompleted_ShouldReturnBadRequest()
    {
        var (reader, progressId, session) = await ArrangeActiveSessionAsync();
        await reader.CompleteSessionAsync(session.Id, pagesRead: 10);
        await reader.WaitForProgressAsync(progressId, expectedCurrentPage: 10);

        var response = await reader.PostAsJsonAsync("Sessions/complete", new CompleteSessionRequest(session.Id, 20));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("Session.NotActive", await response.ReadProblemCodeAsync());
    }

    [Fact]
    public async Task AbandonSession_WhenSessionIsActive_ShouldDropItAndAllowAbandoningOnlyOnce()
    {
        var (reader, _, session) = await ArrangeActiveSessionAsync();

        var abandoned = await reader.AbandonSessionAsync(session.Id);

        Assert.Equal(SessionStatus.Dropped, abandoned.Status);
        Assert.NotNull(abandoned.EndTime);

        var activeResponse = await reader.GetAsync("Sessions/active");
        Assert.Equal(HttpStatusCode.NoContent, activeResponse.StatusCode);

        var secondResponse = await reader.PostAsJsonAsync("Sessions/abandon", new AbandonSessionRequest(session.Id));
        Assert.Equal(HttpStatusCode.BadRequest, secondResponse.StatusCode);
        Assert.Equal("Session.NotActive", await secondResponse.ReadProblemCodeAsync());
    }

    [Fact]
    public async Task AbandonSession_WhenSessionDoesNotExist_ShouldReturnNotFound()
    {
        var (reader, _, _) = await ArrangeActiveSessionAsync();

        var response = await reader.PostAsJsonAsync("Sessions/abandon", new AbandonSessionRequest(Guid.NewGuid()));

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("SessionNotFound", await response.ReadProblemCodeAsync());
    }

    [Fact]
    public async Task Heartbeat_WhenSessionIsActive_ShouldReturnNoContent()
    {
        var (reader, _, session) = await ArrangeActiveSessionAsync();

        var response = await reader.PostAsync("Sessions/heartbeat", null);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        await AssertSessionStillActiveAsync(reader, session.Id);
    }
}
