using MediatR;
using SFA.DAS.CandidateAccount.Data.TrainingCourse;

namespace SFA.DAS.TrainingTypes.Application.Application.Queries.GetTrainingCourses;
public class GetTrainingCoursesQueryHandler(ITrainingCourseRepository TrainingCourseRespository) : IRequestHandler<GetTrainingCoursesQuery, GetTrainingCoursesQueryResult>
{
    public async Task<GetTrainingCoursesQueryResult> Handle(GetTrainingCoursesQuery request, CancellationToken cancellationToken)
    {
        return await TrainingCourseRespository.GetAll(request.ApplicationId, request.CandidateId, cancellationToken);
    }
}
