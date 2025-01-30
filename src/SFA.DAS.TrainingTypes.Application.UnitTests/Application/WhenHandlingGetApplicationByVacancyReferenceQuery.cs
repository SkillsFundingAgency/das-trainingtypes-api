using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.CandidateAccount.Data.Application;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.TrainingTypes.Application.Application.Queries.GetApplicationByVacancyReference;
using SFA.DAS.TrainingTypes.Domain.Application;

namespace SFA.DAS.TrainingTypes.Application.UnitTests.Application;

public class WhenHandlingGetApplicationByVacancyReferenceQuery
{
    [Test, RecursiveMoqAutoData]
    public async Task Then_The_Application_Is_Found_By_Reference_And_Returned(
        GetApplicationByVacancyReferenceQuery query,
        ApplicationEntity entity,
        [Frozen] Mock<IApplicationRepository> repository,
        GetApplicationByVacancyReferenceQueryHandler handler)
    {
        query.CandidateId = entity.CandidateId;
        query.VacancyReference = entity.VacancyReference;

        repository.Setup(x => x.GetByVacancyReference(query.CandidateId, query.VacancyReference)).ReturnsAsync(entity);

        var actual = await handler.Handle(query, CancellationToken.None);

        actual.Application.Should().BeEquivalentTo((Domain.Application.Application)entity, options => options.Excluding(prop => prop.AdditionalQuestions));
    }

    [Test, RecursiveMoqAutoData]
    public async Task Then_If_The_Application_Does_Not_Exist_Then_Null_Returned(
        GetApplicationByVacancyReferenceQuery query,
        [Frozen] Mock<IApplicationRepository> repository,
        GetApplicationByVacancyReferenceQueryHandler handler)
    {
        repository.Setup(x => x.GetByVacancyReference(query.CandidateId, query.VacancyReference)).ReturnsAsync((ApplicationEntity)null!);

        var actual = await handler.Handle(query, CancellationToken.None);

        actual.Application.Should().BeNull();
    }
}