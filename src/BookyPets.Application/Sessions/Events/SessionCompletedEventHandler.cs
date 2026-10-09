using BookyPets.Application.Common.Interfaces;
using BookyPets.Domain.SessionAggregate.Events;
using BookyPets.Shared.Mediator.Abstractions;

namespace BookyPets.Application.Sessions.Events;

public class SessionCompletedEventHandler(
    IProgressesRepository progressesRepository,
    IPetsRepository petsRepository,
    IUnitOfWork unitOfWork) : INotificationHandler<SessionCompletedEvent>
{
    private readonly IProgressesRepository _progressesRepository = progressesRepository;
    private readonly IPetsRepository _petsRepository = petsRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task HandleAsync(SessionCompletedEvent notification, CancellationToken cancellationToken = default)
    {
        var progress = await _progressesRepository.GetProgressAsync(notification.ProgressId) ?? throw new InvalidOperationException();

        var targetPage = notification.PagesRead > int.MaxValue - progress.CurrentPage
            ? int.MaxValue
            : progress.CurrentPage + notification.PagesRead;

        var updateResult = progress.AddPagesRead(targetPage);
        if (!updateResult.IsSuccess)
            throw new InvalidOperationException(updateResult.Error.Description);

        await _progressesRepository.UpdateProgressAsync(progress);

        if (notification.PetId is not null)
        {
            var pet = await _petsRepository.GetPetAsync(notification.PetId.Value) ?? throw new InvalidOperationException();

            pet.GainExperienceFromSession(notification.PagesRead, notification.MinutesRead, notification.Genre);

            await _petsRepository.UpdatePetAsync(pet);
        }

        await _unitOfWork.CommitChangesAsync();
    }
}
