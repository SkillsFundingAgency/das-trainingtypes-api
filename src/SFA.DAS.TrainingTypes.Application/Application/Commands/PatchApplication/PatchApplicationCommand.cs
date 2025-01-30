using MediatR;
using Microsoft.AspNetCore.JsonPatch;

namespace SFA.DAS.TrainingTypes.Application.Application.Commands.PatchApplication;

public class PatchApplicationCommand : IRequest<PatchApplicationCommandResponse>
{
    public Guid Id { get; set; }
    public JsonPatchDocument<Domain.Application.PatchApplication> Patch { get; set; }
    public Guid CandidateId { get; set; }
}