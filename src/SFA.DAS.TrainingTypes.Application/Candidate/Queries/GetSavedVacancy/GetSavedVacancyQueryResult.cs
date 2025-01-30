namespace SFA.DAS.TrainingTypes.Application.Candidate.Queries.GetSavedVacancy
{
    public record GetSavedVacancyQueryResult
    {
        public Guid Id { get; set; }
        public Guid CandidateId { get; set; }
        public string? VacancyReference { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}