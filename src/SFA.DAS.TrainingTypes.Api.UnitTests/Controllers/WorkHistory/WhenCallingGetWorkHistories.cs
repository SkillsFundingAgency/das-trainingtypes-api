using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using SFA.DAS.TrainingTypes.Api.ApiResponses;
using SFA.DAS.TrainingTypes.Api.Controllers;
using SFA.DAS.TrainingTypes.Domain.Application;
using SFA.DAS.TrainingTypes.Application.Application.Queries.GetApplicationWorkHistories;

namespace SFA.DAS.TrainingTypes.Api.UnitTests.Controllers.WorkHistory
{
    public class WhenCallingGetWorkHistories
    {
        [Test, MoqAutoData]
        public async Task Then_The_Command_Is_Sent_To_Mediator_And_Ok_Returned(
        Guid applicationId,
        Guid candidateId,
        WorkHistoryType workHistoryType,
        GetApplicationWorkHistoriesQueryResult response,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] WorkHistoryController controller)
        {
            //Arrange
            mediator.Setup(x => x.Send(It.Is<GetApplicationWorkHistoriesQuery>(
                    c =>
                        c.ApplicationId.Equals(applicationId) &&
                        c.CandidateId.Equals(candidateId) &&
                        c.WorkHistoryType.Equals(workHistoryType)
                ), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            //Act
            var actual = await controller.GetWorkHistories(candidateId, applicationId, workHistoryType) as OkObjectResult;

            //Assert
            using var scope = new AssertionScope();
            actual.Should().NotBeNull();
            actual?.StatusCode.Should().Be((int)HttpStatusCode.OK);
            actual?.Value.Should().BeEquivalentTo((GetFeatureApiResponse)response);
        }

        [Test, MoqAutoData]
        public async Task Then_If_Exception_Returned_From_Mediator_Then_InternalServerError_Is_Returned(
            Guid applicationId,
            Guid candidateId,
            WorkHistoryType workHistoryType,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] WorkHistoryController controller)
        {
            //Arrange
            mediator.Setup(x => x.Send(It.IsAny<GetApplicationWorkHistoriesQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());

            //Act
            var actual = await controller.GetWorkHistories(candidateId, applicationId, workHistoryType) as StatusCodeResult;

            //Assert
            Assert.That(actual, Is.Not.Null);
            actual?.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}
