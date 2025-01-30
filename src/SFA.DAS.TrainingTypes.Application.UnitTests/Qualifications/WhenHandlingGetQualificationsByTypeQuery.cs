using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.CandidateAccount.Data.Qualification;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.TrainingTypes.Application.Application.Queries.GetApplicationQualificationsByType;
using SFA.DAS.TrainingTypes.Domain.Application;

namespace SFA.DAS.TrainingTypes.Application.UnitTests.Qualifications;

public class WhenHandlingGetApplicationQualificationsByTypeQuery
{
    [Test, RecursiveMoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_Data_Returned(
        GetApplicationQualificationsByTypeQuery query,
        List<QualificationEntity> qualifications,
        [Frozen] Mock<IQualificationRepository> repository,
        GetApplicationQualificationsByTypeQueryHandler handler)
    {
        repository.Setup(x => x.GetCandidateApplicationQualificationsByQualificationReferenceType(
                query.CandidateId, query.ApplicationId, query.QualificationReferenceId))
            .ReturnsAsync(qualifications);

        var actual = await handler.Handle(query, CancellationToken.None);

        actual.Qualifications.Should().BeEquivalentTo(qualifications.Select(c => (Qualification)c!).ToList());
    }

    [Test, RecursiveMoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_Empty_List_Returned_If_No_Data(
        GetApplicationQualificationsByTypeQuery query,
        List<QualificationEntity> qualifications,
        [Frozen] Mock<IQualificationRepository> repository,
        GetApplicationQualificationsByTypeQueryHandler handler)
    {
        repository.Setup(x => x.GetCandidateApplicationQualificationsByQualificationReferenceType(
                query.CandidateId, query.ApplicationId, query.QualificationReferenceId))
            .ReturnsAsync([]);

        var actual = await handler.Handle(query, CancellationToken.None);

        actual.Qualifications.Should().BeEmpty();
    }
}