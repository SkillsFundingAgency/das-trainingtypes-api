using SFA.DAS.TrainingTypes.Application.Application.Queries.GetRecognitionOfPriorLearning;

namespace SFA.DAS.TrainingTypes.Api.ApiResponses
{
    public class GetRecognitionOfPriorLearningApiResponse
    {
        public bool IsRequired { get; set; }
        public int? OffTheJobTrainingMinimumHours { get; set; }

        public static implicit operator GetRecognitionOfPriorLearningApiResponse(GetRecognitionOfPriorLearningResult source)
        {
            return new GetRecognitionOfPriorLearningApiResponse
            {
                IsRequired = source.IsRequired,
                OffTheJobTrainingMinimumHours = source.OffTheJobTrainingMinimumHours
            };
        }
    }
}
