using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.TrainingTypes.Api.ApiRequests;
using SFA.DAS.TrainingTypes.Api.ApiResponses;
using SFA.DAS.TrainingTypes.Api.Controllers;
using SFA.DAS.TrainingTypes.Application.Application.Commands.UpsertQualification;
using SFA.DAS.TrainingTypes.Domain.Application;

namespace SFA.DAS.TrainingTypes.Api.UnitTests.Controllers.Qualifications;

public class WhenCallingPutOnQualifications
{
    [Test, MoqAutoData]
    public async Task Then_If_The_Response_IsCreated_Then_Created_Response_Returned(
        Guid candidateId,
        Guid applicationId,
        QualificationRequest request,
        UpsertQualificationCommandResponse response,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] QualificationController controller)
    {
        response.IsCreated = true;
        mediator.Setup(x =>
            x.Send(
                It.Is<UpsertQualificationCommand>(c =>
                    c.CandidateId == candidateId && c.ApplicationId == applicationId
                                                 && c.Qualification.Id == request.Id
                                                 && c.Qualification.Subject == request.Subject
                                                 && c.Qualification.ToYear == request.ToYear
                                                 && c.Qualification.Grade == request.Grade
                                                 && c.Qualification.IsPredicted == request.IsPredicted
                                                 && c.Qualification.AdditionalInformation == request.AdditionalInformation
                                                 ), CancellationToken.None)).ReturnsAsync(response);

        var actual = await controller.Put(candidateId, applicationId, request) as CreatedResult;

        Assert.That(actual, Is.Not.Null);
        var actualValue = actual!.Value as Qualification;
        actualValue.Should().BeEquivalentTo(response.Qualification);
    }

    [Test, MoqAutoData]
    public async Task Then_If_The_Response_IsNotCreated_Then_Ok_Response_Returned(
        Guid candidateId,
        Guid applicationId,
        QualificationRequest request,
        UpsertQualificationCommandResponse response,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] QualificationController controller)
    {
        response.IsCreated = false;
        mediator.Setup(x =>
            x.Send(
                It.Is<UpsertQualificationCommand>(c =>
                    c.CandidateId == candidateId && c.ApplicationId == applicationId
                                                 && c.Qualification.Id == request.Id
                                                 && c.Qualification.Subject == request.Subject
                                                 && c.Qualification.ToYear == request.ToYear
                                                 && c.Qualification.Grade == request.Grade
                                                 && c.Qualification.IsPredicted == request.IsPredicted
                                                 && c.Qualification.AdditionalInformation == request.AdditionalInformation
                ), CancellationToken.None)).ReturnsAsync(response);

        var actual = await controller.Put(candidateId, applicationId, request) as OkObjectResult;

        Assert.That(actual, Is.Not.Null);
        var actualValue = actual!.Value as GetQualificationApiResponse;
        actualValue!.Qualification.Should().BeEquivalentTo(response.Qualification);
    }

    [Test, MoqAutoData]
    public async Task Then_If_Exception_Thrown_InternalServerError_Response_Returned(
        Guid candidateId,
        Guid applicationId,
        QualificationRequest request,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] QualificationController controller)
    {
        mediator.Setup(x =>
            x.Send(
                It.IsAny<UpsertQualificationCommand>(), CancellationToken.None)).ThrowsAsync(new Exception());

        var actual = await controller.Put(candidateId, applicationId, request) as StatusCodeResult;

        Assert.That(actual, Is.Not.Null);
        actual!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}