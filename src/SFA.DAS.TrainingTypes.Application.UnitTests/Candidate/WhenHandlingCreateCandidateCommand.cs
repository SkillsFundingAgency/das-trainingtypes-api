using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.CandidateAccount.Data.Candidate;
using SFA.DAS.CandidateAccount.Data.CandidatePreferences;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.TrainingTypes.Application.Candidate.Commands.CreateCandidate;
using SFA.DAS.TrainingTypes.Domain.Candidate;

namespace SFA.DAS.TrainingTypes.Application.UnitTests.Candidate;

public class WhenHandlingCreateCandidateCommand
{
    [Test, RecursiveMoqAutoData]
    public async Task Then_Request_Is_Handled_And_Entity_Created(
        CreateCandidateCommand command,
        CandidateEntity entity,
        [Frozen] Mock<ICandidateRepository> candidateRepository,
        [Frozen] Mock<ICandidatePreferencesRepository> candidatePreferenceRepository,
        CreateCandidateCommandHandler handler)
    {
        candidateRepository.Setup(x => x.Insert(It.Is<CandidateEntity>(c =>
                c.Email.Equals(command.Email)
                && c.FirstName.Equals(command.FirstName)
                && c.LastName.Equals(command.LastName)
                && c.GovUkIdentifier.Equals(command.GovUkIdentifier)
                && c.PhoneNumber.Equals(command.PhoneNumber)
                && c.MigratedEmail.Equals(command.MigratedEmail)
                && c.MigratedCandidateId.Equals(command.MigratedCandidateId)
                && c.DateOfBirth.Equals(command.DateOfBirth)
                )))
            .ReturnsAsync(new Tuple<CandidateEntity, bool>(entity, true));

        var actual = await handler.Handle(command, CancellationToken.None);

        actual.Candidate.Should().BeEquivalentTo(entity, options => options
            .Excluding(c => c.Applications)
            .Excluding(c => c.Status)
            .Excluding(c => c.Address)
            .Excluding(c => c.AboutYou)
            .Excluding(c => c.CandidatePreferences)
        );

        actual.Candidate.Address.Should().BeEquivalentTo(entity.Address, options => options.Excluding(c => c.Candidate));
        candidatePreferenceRepository.Verify(x => x.Create(entity.Id), Times.Once);
    }

    [Test, RecursiveMoqAutoData]
    public async Task Then_Request_Is_Handled_And_Entity_AlreadyExist_Returned(
        CreateCandidateCommand command,
        CandidateEntity entity,
        [Frozen] Mock<ICandidateRepository> candidateRepository,
        [Frozen] Mock<ICandidatePreferencesRepository> candidatePreferenceRepository,
        CreateCandidateCommandHandler handler)
    {
        candidateRepository.Setup(x => x.Insert(It.Is<CandidateEntity>(c =>
                c.Email.Equals(command.Email)
                && c.FirstName.Equals(command.FirstName)
                && c.LastName.Equals(command.LastName)
                && c.GovUkIdentifier.Equals(command.GovUkIdentifier)
            )))
            .ReturnsAsync(new Tuple<CandidateEntity, bool>(entity, false));

        var actual = await handler.Handle(command, CancellationToken.None);

        actual.Candidate.Should().BeEquivalentTo(entity, options => options
            .Excluding(c => c.Applications)
            .Excluding(c => c.Status)
            .Excluding(c => c.Address)
            .Excluding(c => c.CandidatePreferences)
            .Excluding(c => c.AboutYou)
        );

        actual.Candidate.Address.Should().BeEquivalentTo(entity.Address, options => options.Excluding(c => c.Candidate));

        candidatePreferenceRepository.Verify(x => x.Create(entity.Id), Times.Never);
    }

    [Test, RecursiveMoqAutoData]
    public async Task Then_The_Email_Already_Exist_Is_Handled_And_Entity_AlreadyExist_Returned(
        CreateCandidateCommand command,
        CandidateEntity entity,
        [Frozen] Mock<ICandidateRepository> candidateRepository,
        [Frozen] Mock<ICandidatePreferencesRepository> candidatePreferenceRepository,
        CreateCandidateCommandHandler handler)
    {
        candidateRepository.Setup(x => x.Insert(It.Is<CandidateEntity>(c =>
                c.Email.Equals(command.Email)
            )))
            .ReturnsAsync(new Tuple<CandidateEntity, bool>(entity, false));

        var actual = await handler.Handle(command, CancellationToken.None);

        actual.Candidate.Should().BeEquivalentTo(entity, options => options
            .Excluding(c => c.Applications)
            .Excluding(c => c.Status)
            .Excluding(c => c.Address)
            .Excluding(c => c.CandidatePreferences)
            .Excluding(c => c.AboutYou)
        );

        actual.Candidate.Address.Should().BeEquivalentTo(entity.Address, options => options.Excluding(c => c.Candidate));

        candidatePreferenceRepository.Verify(x => x.Create(entity.Id), Times.Never);
    }
}