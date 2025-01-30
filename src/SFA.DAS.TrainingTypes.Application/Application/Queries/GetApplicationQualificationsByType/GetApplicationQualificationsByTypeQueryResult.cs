using SFA.DAS.TrainingTypes.Domain.Application;

namespace SFA.DAS.TrainingTypes.Application.Application.Queries.GetApplicationQualificationsByType;

public class GetApplicationQualificationsByTypeQueryResult
{
    public required List<Qualification> Qualifications { get; set; }
}