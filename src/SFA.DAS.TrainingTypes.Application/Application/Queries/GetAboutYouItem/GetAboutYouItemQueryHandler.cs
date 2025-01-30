using MediatR;
using SFA.DAS.CandidateAccount.Data.AboutYou;
using SFA.DAS.CandidateAccount.Domain.Candidate;

namespace SFA.DAS.TrainingTypes.Application.Application.Queries.GetAboutYouItem;
public class GetAboutYouItemQueryHandler(IAboutYouRespository aboutYouRespository) : IRequestHandler<GetAboutYouItemQuery, GetAboutYouItemQueryResult>
{
    public async Task<GetAboutYouItemQueryResult> Handle(GetAboutYouItemQuery request, CancellationToken cancellationToken)
    {
        var aboutYou = await aboutYouRespository.Get(request.CandidateId);

        if (aboutYou is null) return new GetAboutYouItemQueryResult();

        return new GetAboutYouItemQueryResult
        {
            AboutYou = aboutYou
        };
    }
}
