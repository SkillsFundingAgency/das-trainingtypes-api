using MediatR;
using SFA.DAS.CandidateAccount.Data.Qualification;
using SFA.DAS.TrainingTypes.Domain.Application;

namespace SFA.DAS.TrainingTypes.Application.Application.Queries.GetApplicationQualificationsByType;

public class GetApplicationQualificationsByTypeQueryHandler(IQualificationRepository qualificationRepository) : IRequestHandler<GetApplicationQualificationsByTypeQuery,
    GetApplicationQualificationsByTypeQueryResult>
{
    public async Task<GetApplicationQualificationsByTypeQueryResult> Handle(GetApplicationQualificationsByTypeQuery request, CancellationToken cancellationToken)
    {
        var result =
            await qualificationRepository.GetCandidateApplicationQualificationsByQualificationReferenceType(
                request.CandidateId, request.ApplicationId, request.QualificationReferenceId);

        return new GetApplicationQualificationsByTypeQueryResult
        {
            Qualifications = result.Select(x => (Qualification)x!).ToList()
        };
    }
}