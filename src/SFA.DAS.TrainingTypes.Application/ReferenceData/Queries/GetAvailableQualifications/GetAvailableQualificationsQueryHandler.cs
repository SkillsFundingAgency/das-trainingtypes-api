using MediatR;
using SFA.DAS.CandidateAccount.Data.ReferenceData;
using SFA.DAS.TrainingTypes.Domain.Application;

namespace SFA.DAS.TrainingTypes.Application.ReferenceData.Queries.GetAvailableQualifications;

public class GetAvailableQualificationsQueryHandler(IQualificationReferenceRepository repository) : IRequestHandler<GetAvailableQualificationsQuery, GetAvailableQualificationsQueryResult>
{
    public async Task<GetAvailableQualificationsQueryResult> Handle(GetAvailableQualificationsQuery request, CancellationToken cancellationToken)
    {
        var data = await repository.GetAll();

        return new GetAvailableQualificationsQueryResult
        {
            QualificationReferences = data.Select(c => (QualificationReference)c).ToList()
        };
    }
}