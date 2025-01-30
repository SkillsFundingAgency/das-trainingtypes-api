using MediatR;
using SFA.DAS.CandidateAccount.Data.Application;
using SFA.DAS.CandidateAccount.Data.TrainingCourse;
using SFA.DAS.TrainingTypes.Domain.Application;


namespace SFA.DAS.TrainingTypes.Application.Application.Commands.DeleteTrainingCourse
{
    public class DeleteTrainingCourseCommandHandler(ITrainingCourseRepository trainingCourseRepository, IApplicationRepository applicationRepository) : IRequestHandler<DeleteTrainingCourseCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteTrainingCourseCommand command, CancellationToken cancellation)
        {
            var application = await applicationRepository.GetById(command.ApplicationId);

            if (application == null || application.CandidateId != command.CandidateId)
            {
                throw new InvalidOperationException($"Application {command.ApplicationId} not found");
            }

            if (application.TrainingCoursesStatus is (short)SectionStatus.PreviousAnswer)
            {
                application.TrainingCoursesStatus = (short)SectionStatus.InProgress;
                await applicationRepository.Update(application);
            }

            await trainingCourseRepository.Delete(command.ApplicationId, command.Id, command.CandidateId);

            return Unit.Value;

        }
    }
}
