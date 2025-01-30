using System.Configuration;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.CandidateAccount.Application.Candidate.Queries.GetSavedVacancies;
using SFA.DAS.CandidateAccount.Data.SavedVacancy;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.TrainingTypes.Application.Candidate.Commands.AddSavedVacancy;
using SFA.DAS.TrainingTypes.Domain.Candidate;

namespace SFA.DAS.TrainingTypes.Application.UnitTests.SavedVacancies
{
    [TestFixture]
    public class WhenHandlingAddSavedVacancyCommand
    {
        [Test, RecursiveMoqAutoData]
        public async Task Then_The_Query_Is_Handled_And_Data_Returned(
            AddSavedVacancyCommand command,
            List<SavedVacancy> savedVacancies,
            Tuple<SavedVacancy, bool> repositoryResult,
            [Frozen] Mock<ISavedVacancyRepository> repository,
            AddSavedVacancyCommandHandler handler)
        {
            repository.Setup(x =>
                    x.Upsert(It.Is<SavedVacancy>(v => v.CandidateId == command.CandidateId && v.VacancyReference == command.VacancyReference)))

                .ReturnsAsync(repositoryResult);

            var actual = await handler.Handle(command, CancellationToken.None);

            actual.SavedVacancy.Should().BeEquivalentTo(repositoryResult.Item1, options => options.ExcludingMissingMembers());
        }
    }
}
