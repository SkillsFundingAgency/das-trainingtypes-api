namespace SFA.DAS.TrainingTypes.Application.Application.Queries.GetApplicationByVacancyReference
{
    public record GetApplicationByVacancyReferenceQueryResult
    {
        public Domain.Application.Application? Application { get; set; }
    }
}
