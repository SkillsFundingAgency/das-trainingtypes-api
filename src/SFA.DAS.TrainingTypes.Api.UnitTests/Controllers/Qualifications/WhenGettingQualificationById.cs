using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.CandidateAccount.Domain.Application;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.TrainingTypes.Api.ApiResponses;
using SFA.DAS.TrainingTypes.Api.Controllers;
using SFA.DAS.TrainingTypes.Application.Application.Queries.GetQualification;

namespace SFA.DAS.TrainingTypes.Api.UnitTests.Controllers.Qualifications;

public class WhenGettingQualificationById
{
    [Test, MoqAutoData]
    public async Task Then_The_Qualification_Is_Returned_By_Id(
        Guid id,
        Guid applicationId,
        Guid candidateId,
        GetQualificationQueryResult response,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] QualificationController controller)
    {
        mediator.Setup(x =>
                x.Send(It.Is<GetQualificationQuery>(c =>
                    c.CandidateId == candidateId && c.ApplicationId == applicationId && c.Id == id), CancellationToken.None))
            .ReturnsAsync(response);

        var actual = await controller.GetById(candidateId, applicationId, id) as OkObjectResult;

        Assert.That(actual, Is.Not.Null);
        var actualModel = actual!.Value as GetQualificationApiResponse;
        actualModel!.Qualification.Should().BeEquivalentTo(response.Qualification);
    }

    [Test, MoqAutoData]
    public async Task Then_If_There_Is_No_Qualification_Returned_By_Id_Not_Found_Returned(
        Guid id,
        Guid applicationId,
        Guid candidateId,
        GetQualificationQueryResult response,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] QualificationController controller)
    {
        mediator.Setup(x =>
                x.Send(It.Is<GetQualificationQuery>(c =>
                    c.CandidateId == candidateId && c.ApplicationId == applicationId && c.Id == id), CancellationToken.None))
            .ReturnsAsync(new GetQualificationQueryResult { Qualification = null });

        var actual = await controller.GetById(candidateId, applicationId, id) as NotFoundResult;

        Assert.That(actual, Is.Not.Null);
        actual.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }

    [Test, MoqAutoData]
    public async Task Then_If_There_Is_An_Exception_Returned_Then_InternalServerError_Response_Returned(
        Guid id,
        Guid applicationId,
        Guid candidateId,
        GetQualificationQueryResult response,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] QualificationController controller)
    {
        mediator.Setup(x =>
                x.Send(It.Is<GetQualificationQuery>(c =>
                    c.CandidateId == candidateId && c.ApplicationId == applicationId && c.Id == id), CancellationToken.None))
            .ThrowsAsync(new Exception());

        var actual = await controller.GetById(candidateId, applicationId, id) as StatusCodeResult;

        Assert.That(actual, Is.Not.Null);
        actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}