using System.ComponentModel.DataAnnotations;
using SFA.DAS.TrainingTypes.Domain.Application;

namespace SFA.DAS.TrainingTypes.Api.ApiRequests;

public class PostApplicationRequest
{
    public required LegacyApplication LegacyApplication { get; set; }
}