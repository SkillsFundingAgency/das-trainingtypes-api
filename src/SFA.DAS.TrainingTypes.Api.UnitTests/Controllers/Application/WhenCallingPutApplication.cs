using System.ComponentModel.DataAnnotations;
using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.TrainingTypes.Api.ApiRequests;
using SFA.DAS.TrainingTypes.Api.Controllers;
using SFA.DAS.TrainingTypes.Application.Application.Commands.UpsertApplication;

namespace SFA.DAS.TrainingTypes.Api.UnitTests.Controllers.Application;

public class WhenCallingPutApplication
{
    [Test, MoqAutoData]
    public async Task Then_If_MediatorCall_Returns_Created_Then_Created_Result_Returned(
        string vacancyReference,
        ApplicationRequest applicationRequest,
        UpsertApplicationCommandResponse upsertApplicationCommandResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] ApplicationController controller)
    {
        //Arrange
        upsertApplicationCommandResult.IsCreated = true;
        mediator.Setup(x => x.Send(It.Is<UpsertApplicationCommand>(c =>
                c.CandidateId.Equals(applicationRequest.CandidateId)
                && c.Status.Equals(applicationRequest.Status)
                && c.DisabilityStatus.Equals(applicationRequest.DisabilityStatus)
                && c.VacancyReference.Equals(vacancyReference)
                && c.IsApplicationQuestionsComplete.Equals(applicationRequest.IsApplicationQuestionsComplete)
                && c.IsDisabilityConfidenceComplete.Equals(applicationRequest.IsDisabilityConfidenceComplete)
                && c.IsEducationHistoryComplete.Equals(applicationRequest.IsEducationHistoryComplete)
                && c.IsInterviewAdjustmentsComplete.Equals(applicationRequest.IsInterviewAdjustmentsComplete)
                && c.IsWorkHistoryComplete.Equals(applicationRequest.IsWorkHistoryComplete)
                && c.IsAdditionalQuestion1Complete.Equals(applicationRequest.IsAdditionalQuestion1Complete)
                && c.IsAdditionalQuestion2Complete.Equals(applicationRequest.IsAdditionalQuestion2Complete)
                && c.AdditionalQuestions.SequenceEqual(applicationRequest.AdditionalQuestions.ToList())
            ), CancellationToken.None))
            .ReturnsAsync(upsertApplicationCommandResult);

        //Act
        var actual = await controller.PutApplication(vacancyReference, applicationRequest);

        //Assert
        var result = actual as CreatedResult;
        var actualResult = result.Value as Domain.Application.Application;
        actualResult.Should().BeEquivalentTo(upsertApplicationCommandResult.Application);
    }

    [Test, MoqAutoData]
    public async Task Then_If_MediatorCall_Returns_NotCreated_Then_Ok_Result_Returned(
        string vacancyReference,
        ApplicationRequest applicationRequest,
        UpsertApplicationCommandResponse upsertApplicationCommandResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] ApplicationController controller)
    {
        //Arrange
        upsertApplicationCommandResult.IsCreated = false;
        mediator.Setup(x => x.Send(It.Is<UpsertApplicationCommand>(c =>
                c.CandidateId.Equals(applicationRequest.CandidateId)
                && c.Status.Equals(applicationRequest.Status)
                && c.DisabilityStatus.Equals(applicationRequest.DisabilityStatus)
                && c.VacancyReference.Equals(vacancyReference)
                && c.IsApplicationQuestionsComplete.Equals(applicationRequest.IsApplicationQuestionsComplete)
                && c.IsDisabilityConfidenceComplete.Equals(applicationRequest.IsDisabilityConfidenceComplete)
                && c.IsEducationHistoryComplete.Equals(applicationRequest.IsEducationHistoryComplete)
                && c.IsInterviewAdjustmentsComplete.Equals(applicationRequest.IsInterviewAdjustmentsComplete)
                && c.IsWorkHistoryComplete.Equals(applicationRequest.IsWorkHistoryComplete)
                && c.IsAdditionalQuestion1Complete.Equals(applicationRequest.IsAdditionalQuestion1Complete)
                && c.IsAdditionalQuestion2Complete.Equals(applicationRequest.IsAdditionalQuestion2Complete)
                && c.AdditionalQuestions.SequenceEqual(applicationRequest.AdditionalQuestions.ToList())
                ), CancellationToken.None))
            .ReturnsAsync(upsertApplicationCommandResult);

        //Act
        var actual = await controller.PutApplication(vacancyReference, applicationRequest);

        //Assert
        var result = actual as OkObjectResult;
        var actualResult = result.Value as Domain.Application.Application;
        actualResult.Should().BeEquivalentTo(upsertApplicationCommandResult.Application);
    }

    [Test, MoqAutoData]
    public async Task Then_If_ValidationError_Then_BadRequest_Response_Returned(
        string vacancyReference,
        ApplicationRequest userProfileRequest,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] ApplicationController controller)
    {
        //Arrange
        mediator.Setup(x => x.Send(It.IsAny<UpsertApplicationCommand>(),
            CancellationToken.None)).ThrowsAsync(new ValidationException("Error"));

        //Act
        var actual = await controller.PutApplication(vacancyReference, userProfileRequest);

        //Assert
        var result = actual as BadRequestResult;
        result?.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
    }

    [Test, MoqAutoData]
    public async Task Then_If_Error_Then_InternalServerError_Response_Returned(
        string vacancyReference,
        ApplicationRequest userProfileRequest,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] ApplicationController controller)
    {
        //Arrange
        mediator.Setup(x => x.Send(It.IsAny<UpsertApplicationCommand>(),
            CancellationToken.None)).ThrowsAsync(new Exception("Error"));

        //Act
        var actual = await controller.PutApplication(vacancyReference, userProfileRequest);

        //Assert
        var result = actual as StatusCodeResult;
        result?.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}