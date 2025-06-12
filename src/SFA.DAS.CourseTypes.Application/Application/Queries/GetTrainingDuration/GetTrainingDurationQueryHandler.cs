using MediatR;
using SFA.DAS.CourseTypes.Domain.Factories;

namespace SFA.DAS.CourseTypes.Application.Application.Queries.GetTrainingDuration;
public class GetTrainingDurationQueryHandler(ICourseTypeFactory courseTypeFactory) : IRequestHandler<GetTrainingDurationQuery, GetTrainingDurationResult>
{
    public async Task<GetTrainingDurationResult> Handle(GetTrainingDurationQuery request, CancellationToken cancellationToken)
    {
        var courseType = courseTypeFactory.Get(request.CourseTypeShortCode);

        return new GetTrainingDurationResult
        {
            MinimumDurationMonths = courseType.TrainingDuration.MinimumDurationMonths,
            MaximumDurationMonths = courseType.TrainingDuration.MaximumDurationMonths
        };
    }
}
