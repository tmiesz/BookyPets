using BookyPets.Application.Sessions.Commands.CompleteSession;
using BookyPets.Application.Sessions.Commands.StartSession;
using BookyPets.Domain.Tests.TestConstants;

namespace Common.Tests.Sessions;

public static class SessionCommandFactory
{
    public static StartSessionCommand CreateStartSessionCommand(
        Guid progressId,
        Guid? petId = null)
    {
        return new StartSessionCommand(
            ProgressId: progressId,
            PetId: petId);
    }

    public static CompleteSessionCommand CreateCompleteSessionCommand(
        Guid sessionId,
        int? pagesRead = null)
    {
        return new CompleteSessionCommand(
            SessionId: sessionId,
            PagesRead: pagesRead ?? Constants.Session.PagesRead);
    }
}
