using MediatR;

namespace SFA.DAS.TrainingTypes.Application.Application.Commands.DeleteQualification;

public class DeleteQualificationCommand : IRequest<Unit>
{
    public Guid CandidateId { get; set; }
    public Guid ApplicationId { get; set; }
    public Guid Id { get; set; }
}