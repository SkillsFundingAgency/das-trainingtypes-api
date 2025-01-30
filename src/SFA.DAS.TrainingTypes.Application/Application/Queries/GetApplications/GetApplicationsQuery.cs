using MediatR;
using SFA.DAS.TrainingTypes.Domain.Application;

namespace SFA.DAS.TrainingTypes.Application.Application.Queries.GetApplications
{
    public class GetApplicationsQuery : IRequest<GetApplicationsQueryResult>
    {
        public Guid CandidateId { get; set; }
        public ApplicationStatus? Status { get; set; }
    }
}
