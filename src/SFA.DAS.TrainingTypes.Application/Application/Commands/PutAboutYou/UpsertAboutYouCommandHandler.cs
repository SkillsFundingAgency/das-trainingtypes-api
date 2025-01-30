using MediatR;
using SFA.DAS.CandidateAccount.Data.AboutYou;

namespace SFA.DAS.TrainingTypes.Application.Application.Commands.PutAboutYou;
public class UpsertAboutYouCommandHandler(IAboutYouRespository aboutYouRepository)
    : IRequestHandler<UpsertAboutYouCommand, UpsertAboutYouCommandResult>
{
    public async Task<UpsertAboutYouCommandResult> Handle(UpsertAboutYouCommand request, CancellationToken cancellationToken)
    {
        var result = await aboutYouRepository.Upsert(request.AboutYou, request.CandidateId);
        return new UpsertAboutYouCommandResult
        {
            AboutYou = result.Item1,
            IsCreated = result.Item2
        };
    }
}
