using MediatR;
using SFA.DAS.TrainingTypes.Domain.Application;

namespace SFA.DAS.TrainingTypes.Application.Application.Commands.UpsertQualification;

public class UpsertQualificationCommand : IRequest<UpsertQualificationCommandResponse>
{
    public required Guid ApplicationId { get; set; }
    public required Qualification Qualification { get; set; }
    public required Guid CandidateId { get; set; }
    public Guid QualificationReferenceId { get; set; }
}