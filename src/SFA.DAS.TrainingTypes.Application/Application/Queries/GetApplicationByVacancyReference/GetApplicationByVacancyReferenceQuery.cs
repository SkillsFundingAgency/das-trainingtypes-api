using MediatR;

namespace SFA.DAS.TrainingTypes.Application.Application.Queries.GetApplicationByVacancyReference
{
    public record GetApplicationByVacancyReferenceQuery : IRequest<GetApplicationByVacancyReferenceQueryResult>
    {
        public Guid CandidateId { get; set; }
        public required string VacancyReference { get; set; }
    }
}
