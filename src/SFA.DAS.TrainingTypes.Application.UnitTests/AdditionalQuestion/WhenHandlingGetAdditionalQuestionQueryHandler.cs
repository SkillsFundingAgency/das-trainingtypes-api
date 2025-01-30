using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.CandidateAccount.Data.AdditionalQuestion;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.TrainingTypes.Application.Application.Queries.GetAdditionalQuestion;
using SFA.DAS.TrainingTypes.Domain.Application;

namespace SFA.DAS.TrainingTypes.Application.UnitTests.AdditionalQuestion;

[TestFixture]
public class WhenHandlingGetAdditionalQuestionQueryHandler
{
    [Test, RecursiveMoqAutoData]
    public async Task Then_Request_Is_Handled_And_Entities_Returned(
        GetAdditionalQuestionItemQuery request,
        AdditionalQuestionEntity entity,
        [Frozen] Mock<IAdditionalQuestionRepository> additionalQuestionRepository,
        GetAdditionalQuestionItemQueryHandler handler)
    {
        additionalQuestionRepository.Setup(x => x.Get(request.ApplicationId, request.CandidateId, request.Id, CancellationToken.None)).ReturnsAsync(entity);

        var actual = await handler.Handle(request, CancellationToken.None);

        actual.Should().BeEquivalentTo(entity, options => options.Excluding(c => c.ApplicationEntity));
    }
}