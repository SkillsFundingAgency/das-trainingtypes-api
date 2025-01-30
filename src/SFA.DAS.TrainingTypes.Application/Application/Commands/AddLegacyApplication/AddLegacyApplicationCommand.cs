using MediatR;
using SFA.DAS.TrainingTypes.Domain.Application;

namespace SFA.DAS.TrainingTypes.Application.Application.Commands.AddLegacyApplication
{
    public class AddLegacyApplicationCommand : IRequest<AddLegacyApplicationCommandResponse>
    {
        public LegacyApplication LegacyApplication { get; set; } = null!;
    }
}
