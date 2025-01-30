using MediatR;
using SFA.DAS.CandidateAccount.Data.AdditionalQuestion;
using SFA.DAS.CandidateAccount.Data.Application;
using SFA.DAS.CandidateAccount.Data.SavedVacancy;
using SFA.DAS.TrainingTypes.Domain.Application;

namespace SFA.DAS.TrainingTypes.Application.Application.Commands.UpsertApplication;

public class UpsertApplicationCommandHandler(
    IApplicationRepository applicationRepository,
    IAdditionalQuestionRepository additionalQuestionRepository,
    ISavedVacancyRepository savedVacancyRepository)
    : IRequestHandler<UpsertApplicationCommand, UpsertApplicationCommandResponse>
{
    public async Task<UpsertApplicationCommandResponse> Handle(UpsertApplicationCommand command, CancellationToken cancellationToken)
    {
        if (!await applicationRepository.Exists(command.CandidateId, command.VacancyReference))
        {
            var previousApplications = await applicationRepository.GetByCandidateId(command.CandidateId, null);
            var previousApplication = previousApplications.Where(x =>
                x.Status != (short)ApplicationStatus.Draft &&
                x.Status != (short)ApplicationStatus.Withdrawn &&
                x.Status != (short)ApplicationStatus.Expired)
                .Where(x => x.MigrationDate == null)
                .MaxBy(x => x.CreatedDate);

            if (previousApplication != null)
            {
                var requiresDisabilityConfidence = command.IsDisabilityConfidenceComplete == SectionStatus.NotStarted;
                var result = await applicationRepository.Clone(previousApplication.Id, command.VacancyReference, requiresDisabilityConfidence, command.IsAdditionalQuestion1Complete, command.IsAdditionalQuestion2Complete);

                await UpsertAdditionalQuestions(command, cancellationToken, result);
                await RemoveSavedVacancy(command.CandidateId, command.VacancyReference);

                return new UpsertApplicationCommandResponse
                {
                    Application = result,
                    IsCreated = true
                };
            }
        }

        var application = await applicationRepository.Upsert(new ApplicationEntity
        {
            VacancyReference = command.VacancyReference,
            CandidateId = command.CandidateId,
            Status = (short)command.Status,
            DisabilityConfidenceStatus = (short)command.IsDisabilityConfidenceComplete,
            JobsStatus = (short)command.IsApplicationQuestionsComplete,
            QualificationsStatus = (short)command.IsEducationHistoryComplete,
            WorkExperienceStatus = (short)command.IsInterviewAdjustmentsComplete,
            TrainingCoursesStatus = (short)command.IsWorkHistoryComplete,
            AdditionalQuestion1Status = (short)command.IsAdditionalQuestion1Complete,
            AdditionalQuestion2Status = (short)command.IsAdditionalQuestion2Complete,
            DisabilityStatus = command.DisabilityStatus
        });

        await UpsertAdditionalQuestions(command, cancellationToken, application.Item1);
        if (application.Item2)
        {
            await RemoveSavedVacancy(command.CandidateId, command.VacancyReference);
        }

        return new UpsertApplicationCommandResponse
        {
            Application = application.Item1,
            IsCreated = application.Item2
        };
    }

    private async Task UpsertAdditionalQuestions(UpsertApplicationCommand command, CancellationToken cancellationToken, ApplicationEntity application)
    {
        var hasAdditionalQuestions =
            await additionalQuestionRepository.GetAll(application.Id, command.CandidateId, cancellationToken);

        if (hasAdditionalQuestions.Count > 0)
        {
            return;
        }

        foreach (var additionalQuestion in command.AdditionalQuestions)
        {
            await additionalQuestionRepository.UpsertAdditionalQuestion(new TrainingType
            {
                Id = Guid.NewGuid(),
                ApplicationId = application.Id,
                CandidateId = command.CandidateId,
                QuestionText = additionalQuestion,
                Answer = string.Empty,
            }, command.CandidateId);
        }
    }

    private async Task RemoveSavedVacancy(Guid candidateId, string vacancyReference)
    {
        var savedVacancy = await savedVacancyRepository.Get(candidateId, vacancyReference);

        if (savedVacancy != null)
        {
            await savedVacancyRepository.Delete(savedVacancy);
        }
    }
}