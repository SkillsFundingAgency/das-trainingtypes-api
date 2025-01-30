using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.TrainingTypes.Api.ApiResponses;
using SFA.DAS.TrainingTypes.Api.Controllers;
using SFA.DAS.TrainingTypes.Application.Application.Queries.GetWorkHistoryItem;
using SFA.DAS.TrainingTypes.Domain.Application;

namespace SFA.DAS.TrainingTypes.Api.UnitTests.Controllers.WorkHistory
{
    [TestFixture]
    public class WhenCallingGetWorkHistoryItem
    {
        [Test, MoqAutoData]
        public async Task Then_The_Response_Is_Returned_As_Expected(
            Guid applicationId,
            Guid candidateId,
            Guid id,
            WorkHistoryType workHistoryType,
            GetWorkHistoryItemQueryResult response,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] WorkHistoryController controller)
        {
            //Arrange
            mediator.Setup(x => x.Send(It.Is<GetWorkHistoryItemQuery>(
                    c =>
                        c.ApplicationId.Equals(applicationId) &&
                        c.CandidateId.Equals(candidateId) &&
                        c.Id.Equals(id) &&
                        c.WorkHistoryType.Equals(workHistoryType)
                ), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            //Act
            var actual = await controller.Get(candidateId, applicationId, id, workHistoryType) as OkObjectResult;

            //Assert
            using var scope = new AssertionScope();
            actual.Should().NotBeNull();
            actual?.StatusCode.Should().Be((int)HttpStatusCode.OK);
            actual?.Value.Should().BeEquivalentTo((GetWorkHistoryItemApiResponse)response);
        }

        [Test, MoqAutoData]
        public async Task And_Response_Is_Null_Then_Return_NotFound(
            Guid applicationId,
            Guid candidateId,
            Guid id,
            WorkHistoryType workHistoryType,
            GetWorkHistoryItemQueryResult response,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] WorkHistoryController controller)
        {
            mediator.Setup(x => x.Send(It.Is<GetWorkHistoryItemQuery>(
                    c =>
                        c.ApplicationId.Equals(applicationId) &&
                        c.CandidateId.Equals(candidateId) &&
                        c.Id.Equals(id) &&
                        c.WorkHistoryType.Equals(workHistoryType)
                ), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => null);

            var actual = await controller.Get(candidateId, applicationId, id, workHistoryType);

            actual.Should().BeOfType<NotFoundResult>();
        }
    }
}
