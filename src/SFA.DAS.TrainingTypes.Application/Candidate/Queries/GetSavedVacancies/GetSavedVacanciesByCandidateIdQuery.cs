using MediatR;
using SFA.DAS.CandidateAccount.Data.SavedVacancy;

namespace SFA.DAS.TrainingTypes.Application.Candidate.Queries.GetSavedVacancies
{
    public class GetSavedVacanciesByCandidateIdQuery : IRequest<GetSavedVacanciesByCandidateIdQueryResult>
    {
        public Guid CandidateId { get; set; }
    }

    public class GetSavedVacanciesByCandidateIdQueryResult
    {
        public List<SavedVacancy> SavedVacancies { get; set; } = [];

        public class SavedVacancy
        {
            public Guid Id { get; set; }
            public Guid CandidateId { get; set; }
            public string VacancyReference { get; set; }
            public DateTime CreatedOn { get; set; }
        }

    }
    public class GetSavedVacanciesByCandidateIdQueryHandler(ISavedVacancyRepository repository) : IRequestHandler<GetSavedVacanciesByCandidateIdQuery, GetSavedVacanciesByCandidateIdQueryResult>
    {
        public async Task<GetSavedVacanciesByCandidateIdQueryResult> Handle(GetSavedVacanciesByCandidateIdQuery request, CancellationToken cancellationToken)
        {
            var result = await repository.GetByCandidateId(request.CandidateId);

            return new GetSavedVacanciesByCandidateIdQueryResult
            {
                SavedVacancies = result.Select(x => new GetSavedVacanciesByCandidateIdQueryResult.SavedVacancy
                {
                    Id = x.Id,
                    CandidateId = x.CandidateId,
                    VacancyReference = x.VacancyReference,
                    CreatedOn = x.CreatedOn
                }).ToList()
            };
        }
    }
}
