using MediatR;
using SFA.DAS.CourseTypes.Domain.Factories;

namespace SFA.DAS.CourseTypes.Application.Application.Queries.GetRecognitionOfPriorLearning;
public class GetRecognitionOfPriorLearningQueryHandler(ICourseTypeFactory courseTypeFactory) : IRequestHandler<GetRecognitionOfPriorLearningQuery, GetRecognitionOfPriorLearningResult>
{
    public async Task<GetRecognitionOfPriorLearningResult> Handle(GetRecognitionOfPriorLearningQuery request, CancellationToken cancellationToken)
    {
        var courseType = courseTypeFactory.Get(request.CourseTypeShortCode);

        return new GetRecognitionOfPriorLearningResult
        {
            IsRequired = courseType.RecognitionOfPriorLearning.IsRequired,
            OffTheJobTrainingMinimumHours = courseType.RecognitionOfPriorLearning.OffTheJobTrainingMinimumHours
        };
    }
}
