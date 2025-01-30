using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.CandidateAccount.Data.Qualification;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.TrainingTypes.Application.Application.Queries.GetQualifications;
using SFA.DAS.TrainingTypes.Domain.Application;

namespace SFA.DAS.TrainingTypes.Application.UnitTests.Qualifications;

public class WhenHandlingGetApplicationQualificationsQuery
{
    [Test, RecursiveMoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_Data_Returned(
        GetApplicationQualificationsQuery query,
        List<QualificationEntity> qualifications,
        [Frozen] Mock<IQualificationRepository> repository,
        GetApplicationQualificationsQueryHandler handler)
    {
        repository.Setup(x => x.GetCandidateApplicationQualifications(query.CandidateId, query.ApplicationId))
            .ReturnsAsync(qualifications);

        var actual = await handler.Handle(query, CancellationToken.None);

        actual.Qualifications.Should().BeEquivalentTo(qualifications.Select(c => (Qualification)c!).ToList());
    }

    [Test, RecursiveMoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_Empty_List_Returned_If_No_Data(
        GetApplicationQualificationsQuery query,
        List<QualificationEntity> qualifications,
        [Frozen] Mock<IQualificationRepository> repository,
        GetApplicationQualificationsQueryHandler handler)
    {
        repository.Setup(x => x.GetCandidateApplicationQualifications(query.CandidateId, query.ApplicationId))
            .ReturnsAsync([]);

        var actual = await handler.Handle(query, CancellationToken.None);

        actual.Qualifications.Should().BeEmpty();
    }
}