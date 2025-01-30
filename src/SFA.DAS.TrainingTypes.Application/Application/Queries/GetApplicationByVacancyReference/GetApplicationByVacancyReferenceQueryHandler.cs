using MediatR;
using SFA.DAS.CandidateAccount.Data.Application;

namespace SFA.DAS.TrainingTypes.Application.Application.Queries.GetApplicationByVacancyReference
{
    public record GetApplicationByVacancyReferenceQueryHandler(IApplicationRepository ApplicationRepository)
        : IRequestHandler<GetApplicationByVacancyReferenceQuery, GetApplicationByVacancyReferenceQueryResult>
    {
        public async Task<GetApplicationByVacancyReferenceQueryResult> Handle(GetApplicationByVacancyReferenceQuery request, CancellationToken cancellationToken)
        {
            var applicationEntity = await ApplicationRepository.GetByVacancyReference(request.CandidateId, request.VacancyReference);

            if (applicationEntity == null)
            {
                return new GetApplicationByVacancyReferenceQueryResult();
            }

            return new GetApplicationByVacancyReferenceQueryResult
            {
                Application = applicationEntity
            };
        }
    }
}
