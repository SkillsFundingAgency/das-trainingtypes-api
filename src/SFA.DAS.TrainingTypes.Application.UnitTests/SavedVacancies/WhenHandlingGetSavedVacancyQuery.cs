using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.CandidateAccount.Data.SavedVacancy;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.TrainingTypes.Application.Candidate.Queries.GetSavedVacancy;
using SFA.DAS.TrainingTypes.Domain.Candidate;

namespace SFA.DAS.TrainingTypes.Application.UnitTests.SavedVacancies
{
    [TestFixture]
    public class WhenHandlingGetSavedVacancyQuery
    {
        [Test, RecursiveMoqAutoData]
        public async Task Then_The_Query_Is_Handled_And_Data_Returned(
            GetSavedVacancyQuery request,
            SavedVacancy repositoryResult,
            [Frozen] Mock<ISavedVacancyRepository> repository,
            GetSavedVacancyQueryHandler handler)
        {
            repository.Setup(x =>
                    x.Get(request.CandidateId, request.VacancyReference))
                .ReturnsAsync(repositoryResult);

            var actual = await handler.Handle(request, CancellationToken.None);

            actual.Should().BeEquivalentTo(repositoryResult, options => options.ExcludingMissingMembers());
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_The_Query_Is_Handled_And_Empty_Data_Returned(
            GetSavedVacancyQuery request,
            [Frozen] Mock<ISavedVacancyRepository> repository,
            GetSavedVacancyQueryHandler handler)
        {
            repository.Setup(x =>
                    x.Get(request.CandidateId, request.VacancyReference))
                .ReturnsAsync(() => null);

            var actual = await handler.Handle(request, CancellationToken.None);

            actual.Should().BeEquivalentTo(new GetSavedVacancyQueryResult(), options => options.ExcludingMissingMembers());
        }
    }
}
