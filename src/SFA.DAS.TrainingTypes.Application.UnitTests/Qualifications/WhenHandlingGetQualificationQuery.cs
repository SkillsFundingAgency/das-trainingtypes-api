using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.CandidateAccount.Data.Qualification;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.TrainingTypes.Application.Application.Queries.GetQualification;
using SFA.DAS.TrainingTypes.Domain.Application;

namespace SFA.DAS.TrainingTypes.Application.UnitTests.Qualifications;

public class WhenHandlingGetQualificationQuery
{
    [Test, RecursiveMoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_Data_Returned(
        GetQualificationQuery query,
        QualificationEntity qualificationEntity,
        [Frozen] Mock<IQualificationRepository> repository,
        GetQualificationQueryHandler handler)
    {
        repository.Setup(x =>
                x.GetCandidateApplicationQualificationById(query.CandidateId, query.ApplicationId, query.Id))
            .ReturnsAsync(qualificationEntity);

        var actual = await handler.Handle(query, CancellationToken.None);

        actual.Qualification.Should().BeEquivalentTo(qualificationEntity, options => options.ExcludingMissingMembers());
    }
    [Test, RecursiveMoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_Null_Returned_If_Not_Matched(
        GetQualificationQuery query,
        QualificationEntity qualificationEntity,
        [Frozen] Mock<IQualificationRepository> repository,
        GetQualificationQueryHandler handler)
    {
        repository.Setup(x =>
                x.GetCandidateApplicationQualificationById(query.CandidateId, query.ApplicationId, query.Id))
            .ReturnsAsync((QualificationEntity?)null);

        var actual = await handler.Handle(query, CancellationToken.None);

        actual.Qualification.Should().BeNull();
    }
}