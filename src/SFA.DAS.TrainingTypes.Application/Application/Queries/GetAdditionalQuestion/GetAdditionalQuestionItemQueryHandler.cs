using MediatR;
using SFA.DAS.CandidateAccount.Data.AdditionalQuestion;

namespace SFA.DAS.TrainingTypes.Application.Application.Queries.GetAdditionalQuestion
{
    public class GetAdditionalQuestionItemQueryHandler(IAdditionalQuestionRepository additionalQuestionRepository) : IRequestHandler<GetAdditionalQuestionItemQuery, GetAdditionalQuestionItemQueryResult?>
    {
        public async Task<GetAdditionalQuestionItemQueryResult?> Handle(GetAdditionalQuestionItemQuery request, CancellationToken cancellationToken)
        {
            var result = await additionalQuestionRepository.Get(request.ApplicationId, request.CandidateId, request.Id, cancellationToken);
            return result == null ? null : (GetAdditionalQuestionItemQueryResult)result;
        }
    }
}
