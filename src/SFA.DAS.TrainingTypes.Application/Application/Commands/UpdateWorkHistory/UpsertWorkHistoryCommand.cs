using MediatR;
using SFA.DAS.TrainingTypes.Domain.Application;

namespace SFA.DAS.TrainingTypes.Application.Application.Commands.UpdateWorkHistory
{
    public class UpsertWorkHistoryCommand : IRequest<UpsertWorkHistoryCommandResponse>
    {
        public required Guid ApplicationId { get; set; }
        public required WorkHistory WorkHistory { get; set; }
        public required Guid CandidateId { get; set; }
    }
}
