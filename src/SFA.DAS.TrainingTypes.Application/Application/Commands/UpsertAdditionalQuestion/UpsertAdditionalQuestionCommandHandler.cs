using MediatR;
using SFA.DAS.CandidateAccount.Data.AdditionalQuestion;

namespace SFA.DAS.TrainingTypes.Application.Application.Commands.UpsertAdditionalQuestion;

public record UpsertAdditionalQuestionCommandHandler(IAdditionalQuestionRepository AdditionalQuestionRepository)
    : IRequestHandler<UpsertAdditionalQuestionCommand, UpsertAdditionalQuestionCommandResponse>
{
    public async Task<UpsertAdditionalQuestionCommandResponse> Handle(UpsertAdditionalQuestionCommand request, CancellationToken cancellationToken)
    {
        var result = await AdditionalQuestionRepository.UpsertAdditionalQuestion(request.AdditionalQuestion, request.CandidateId);
        return new UpsertAdditionalQuestionCommandResponse
        {
            AdditionalQuestion = result.Item1,
            IsCreated = result.Item2
        };
    }
}