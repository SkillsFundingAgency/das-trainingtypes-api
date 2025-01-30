using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.CandidateAccount.Application.Application.Commands.UpdateWorkHistory;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using SFA.DAS.TrainingTypes.Api.Controllers;
using SFA.DAS.TrainingTypes.Api.ApiRequests;
using SFA.DAS.TrainingTypes.Application.Application.Commands.UpsertAdditionalQuestion;

namespace SFA.DAS.TrainingTypes.Api.UnitTests.Controllers.AdditionalQuestion;

[TestFixture]
public class WhenCallingPutAdditionalQuestion
{
    [Test, MoqAutoData]
    public async Task Then_If_MediatorCall_Returns_Created_Then_Created_Result_Returned(
       Guid id,
       Guid candidateId,
       Guid applicationId,
       AdditionalQuestionRequest upsertWorkHistoryRequest,
       UpsertAdditionalQuestionCommandResponse upsertAdditionalQuestionCommandResponse,
       [Frozen] Mock<IMediator> mediator,
       [Greedy] AdditionalQuestionController controller)
    {
        upsertAdditionalQuestionCommandResponse.IsCreated = true;
        mediator.Setup(x => x.Send(It.Is<UpsertAdditionalQuestionCommand>(c =>
                c.CandidateId == candidateId
                && c.AdditionalQuestion.ApplicationId.Equals(applicationId)
                && c.AdditionalQuestion.CandidateId.Equals(candidateId)
                && c.AdditionalQuestion.Answer!.Equals(upsertWorkHistoryRequest.Answer)
                && c.AdditionalQuestion.Id.Equals(id)
            ), CancellationToken.None))
            .ReturnsAsync(upsertAdditionalQuestionCommandResponse);

        var actual = await controller.PutAdditionalQuestionItem(candidateId, applicationId, id, upsertWorkHistoryRequest);
        var result = actual as CreatedResult;
        var actualResult = result.Value as Domain.Application.TrainingType;

        actual.Should().BeOfType<CreatedResult>();
        actualResult.Should().BeEquivalentTo(upsertAdditionalQuestionCommandResponse.AdditionalQuestion);
    }

    [Test, MoqAutoData]
    public async Task Then_If_MediatorCall_Returns_NotCreated_Then_Ok_Result_Returned(
        Guid id,
        Guid candidateId,
        Guid applicationId,
        AdditionalQuestionRequest upsertWorkHistoryRequest,
        UpsertAdditionalQuestionCommandResponse upsertAdditionalQuestionCommandResponse,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] AdditionalQuestionController controller)
    {
        upsertAdditionalQuestionCommandResponse.IsCreated = false;
        mediator.Setup(x => x.Send(It.Is<UpsertAdditionalQuestionCommand>(c =>
                c.CandidateId == candidateId
                && c.AdditionalQuestion.ApplicationId.Equals(applicationId)
                && c.AdditionalQuestion.CandidateId.Equals(candidateId)
                && c.AdditionalQuestion.Answer!.Equals(upsertWorkHistoryRequest.Answer)
                && c.AdditionalQuestion.Id.Equals(id)
            ), CancellationToken.None))
            .ReturnsAsync(upsertAdditionalQuestionCommandResponse);

        var actual = await controller.PutAdditionalQuestionItem(candidateId, applicationId, id, upsertWorkHistoryRequest);
        var result = actual as OkObjectResult;
        var actualResult = result.Value as Domain.Application.TrainingType;

        actual.Should().BeOfType<OkObjectResult>();
        actualResult.Should().BeEquivalentTo(upsertAdditionalQuestionCommandResponse.AdditionalQuestion);
    }

    [Test, MoqAutoData]
    public async Task Then_If_Error_Then_InternalServerError_Response_Returned(
        AdditionalQuestionRequest upsertWorkHistoryRequest,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] AdditionalQuestionController controller)
    {
        mediator.Setup(x => x.Send(It.IsAny<UpsertAdditionalQuestionCommand>(),
            CancellationToken.None)).ThrowsAsync(new Exception("Error"));

        var actual = await controller.PutAdditionalQuestionItem(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), upsertWorkHistoryRequest);

        actual.As<StatusCodeResult>().StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}