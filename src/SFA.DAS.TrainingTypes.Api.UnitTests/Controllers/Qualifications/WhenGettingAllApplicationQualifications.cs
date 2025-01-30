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
using SFA.DAS.TrainingTypes.Application.Application.Queries.GetApplicationQualificationsByType;
using SFA.DAS.TrainingTypes.Application.Application.Queries.GetQualifications;

namespace SFA.DAS.TrainingTypes.Api.UnitTests.Controllers.Qualifications;

public class WhenGettingAllApplicationQualifications
{
    [Test, MoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_Data_Returned(
        Guid applicationId,
        Guid candidateId,
        GetApplicationQualificationsQueryResult response,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] QualificationController controller)
    {
        mediator.Setup(x =>
                x.Send(
                    It.Is<GetApplicationQualificationsQuery>(c =>
                        c.ApplicationId == applicationId && c.CandidateId == candidateId), CancellationToken.None))
            .ReturnsAsync(response);

        var actual = await controller.GetAll(candidateId, applicationId) as OkObjectResult;

        Assert.That(actual, Is.Not.Null);
        var actualModel = actual!.Value as GetQualificationsApiResponse;
        actualModel!.Qualifications.Should().BeEquivalentTo(response.Qualifications);
    }

    [Test, MoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_Data_Returned_When_Filtering_By_Type(
        Guid applicationId,
        Guid candidateId,
        Guid qualificationReferenceId,
        GetApplicationQualificationsByTypeQueryResult response,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] QualificationController controller)
    {
        mediator.Setup(x =>
                x.Send(
                    It.Is<GetApplicationQualificationsByTypeQuery>(c =>
                        c.ApplicationId == applicationId && c.CandidateId == candidateId && c.QualificationReferenceId == qualificationReferenceId), CancellationToken.None))
            .ReturnsAsync(response);

        var actual = await controller.GetAll(candidateId, applicationId, qualificationReferenceId) as OkObjectResult;

        Assert.That(actual, Is.Not.Null);
        var actualModel = actual!.Value as GetQualificationsApiResponse;
        actualModel!.Qualifications.Should().BeEquivalentTo(response.Qualifications);

    }

    [Test, MoqAutoData]
    public async Task Then_If_Exception_Thrown_InternalServerError_Response_Returned(
        Guid applicationId,
        Guid candidateId,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] QualificationController controller)
    {
        mediator.Setup(x =>
                x.Send(
                    It.Is<GetApplicationQualificationsQuery>(c =>
                        c.ApplicationId == applicationId && c.CandidateId == candidateId), CancellationToken.None))
            .ThrowsAsync(new Exception());

        var actual = await controller.GetAll(candidateId, applicationId) as StatusCodeResult;

        Assert.That(actual, Is.Not.Null);
        actual!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}