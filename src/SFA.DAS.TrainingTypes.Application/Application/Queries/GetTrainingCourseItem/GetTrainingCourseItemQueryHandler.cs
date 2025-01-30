using MediatR;
using SFA.DAS.CandidateAccount.Data.TrainingCourse;

namespace SFA.DAS.TrainingTypes.Application.Application.Queries.GetTrainingCourseItem;
public class GetTrainingCourseItemQueryHandler(ITrainingCourseRepository trainingCourseRepository) : IRequestHandler<GetTrainingCourseItemQuery, GetTrainingCourseItemQueryResult?>
{
    public async Task<GetTrainingCourseItemQueryResult?> Handle(GetTrainingCourseItemQuery request, CancellationToken cancellationToken)
    {
        var result = await trainingCourseRepository.Get(request.ApplicationId, request.CandidateId, request.Id, cancellationToken);
        return result == null ? null : (GetTrainingCourseItemQueryResult)result;
    }
}
