using MediatR;
using SFA.DAS.CandidateAccount.Data.Application;
using SFA.DAS.CandidateAccount.Data.TrainingCourse;
using SFA.DAS.TrainingTypes.Domain.Application;

namespace SFA.DAS.TrainingTypes.Application.Application.Commands.UpsertTrainingCourse;
public class UpsertTrainingCourseCommandHandler(ITrainingCourseRepository trainingCourseRepository, IApplicationRepository applicationRepository)
    : IRequestHandler<UpsertTrainingCourseCommand, UpsertTrainingCourseCommandResponse>
{
    public async Task<UpsertTrainingCourseCommandResponse> Handle(UpsertTrainingCourseCommand request, CancellationToken cancellationToken)
    {
        var application = await applicationRepository.GetById(request.ApplicationId);
        if (application == null || application.CandidateId != request.CandidateId)
        {
            throw new InvalidOperationException($"Application {request.ApplicationId} not found");
        }

        var result = await trainingCourseRepository.UpsertTrainingCourse(request.TrainingCourse, request.CandidateId);

        if (application.TrainingCoursesStatus is (short)SectionStatus.NotStarted or (short)SectionStatus.PreviousAnswer)
        {
            application.TrainingCoursesStatus = (short)SectionStatus.InProgress;
            await applicationRepository.Update(application);
        }

        return new UpsertTrainingCourseCommandResponse
        {
            TrainingCourse = result.Item1,
            IsCreated = result.Item2
        };
    }
}
