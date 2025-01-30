using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.CandidateAccount.Data.Candidate;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.TrainingTypes.Application.Candidate.Commands.DeleteCandidate;
using SFA.DAS.TrainingTypes.Domain.Application;
using SFA.DAS.TrainingTypes.Domain.Candidate;

namespace SFA.DAS.TrainingTypes.Application.UnitTests.Candidate
{
    [TestFixture]
    public class WhenHandlingDeleteCandidateCommand
    {
        [Test, RecursiveMoqAutoData]
        public async Task Then_Request_Is_Handled_And_Entity_Updated(
            DeleteCandidateCommand command,
            CandidateEntity entity,
            [Frozen] Mock<ICandidateRepository> candidateRepository,
            DeleteCandidateCommandHandler handler)
        {
            entity.Status = (short)CandidateStatus.Deleted;
            entity.GovUkIdentifier = null;
            candidateRepository.Setup(x => x.DeleteCandidate(command.CandidateId))!
                .ReturnsAsync(new Tuple<CandidateEntity, bool>(entity, true));

            var actual = await handler.Handle(command, CancellationToken.None);

            actual.Candidate.Should().BeEquivalentTo(entity, options => options
                .Excluding(c => c.GovUkIdentifier)
                .Excluding(c => c.Applications)
                .Excluding(c => c.Status)
                .Excluding(c => c.Address)
                .Excluding(c => c.AboutYou)
                .Excluding(c => c.CandidatePreferences)
            );

            actual.Candidate!.Status.Should().Be(CandidateStatus.Deleted);
            actual.Candidate.GovUkIdentifier.Should().BeNull();
            actual.Candidate.Address.Should().BeEquivalentTo(entity.Address, options => options.Excluding(c => c.Candidate));
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_Request_Is_Handled_And_Entity_Does_Not_Exist_Returned(
            DeleteCandidateCommand command,
            CandidateEntity entity,
            [Frozen] Mock<ICandidateRepository> candidateRepository,
            DeleteCandidateCommandHandler handler)
        {
            candidateRepository.Setup(x => x.DeleteCandidate(command.CandidateId))!
                 .ReturnsAsync(new Tuple<CandidateEntity, bool>(entity, false));

            var actual = await handler.Handle(command, CancellationToken.None);

            actual.Candidate.Should().BeNull();
        }
    }
}
