using SFA.DAS.TrainingTypes.Domain.Application;

namespace SFA.DAS.TrainingTypes.Application.Application.Queries.GetQualifications;

public class GetApplicationQualificationsQueryResult
{
    public required List<Qualification> Qualifications { get; set; }
}