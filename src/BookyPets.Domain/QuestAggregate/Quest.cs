using BookyPets.Domain.QuestAggregate;
using BookyPets.Domain.Common;
using BookyPets.Shared.Result;
using BookyPets.Domain.SessionAggregate.Events;

namespace BookyPets.Domain.QuestAggregate;

public class Quest : AggregateRoot
{
    private readonly Guid _readerId;
    private readonly Guid _questBaseId;

    public int CurrentProgress { get; private set; }
    public QuestRequirement Requirement { get; private set; }

    public QuestStatus Status { get; private set; }

    public Quest(Guid readerId, Guid questTemplateId, QuestRequirement questRequirement, Guid? id = null) : base(id ?? Guid.NewGuid())
    {
        _readerId = readerId;
        _questBaseId = questTemplateId;
        Requirement = questRequirement;
        Status = QuestStatus.Inactive;
    }

    public Result UpdateProgress(SessionCompletedEvent e)
    {
        if (Status == QuestStatus.Completed)
            return QuestErrors.AlreadyCompleted;
        if (Status != QuestStatus.Active)
            return QuestErrors.NotActive;

        var gainedProgress = Requirement.ExtractProgress(e);

        if (gainedProgress == 0)
            return Result.Success;

        CurrentProgress += gainedProgress;

        if (Requirement.IsCompleted(CurrentProgress))
        {
            Status = QuestStatus.Completed;
            // _domainEvents.Add(new QuestCompletedEvent());
        }

        return Result.Success;
    }

}
