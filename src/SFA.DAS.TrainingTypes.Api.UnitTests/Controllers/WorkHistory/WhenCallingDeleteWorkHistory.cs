using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.CandidateAccount.Api.ApiRequests;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.TrainingTypes.Api.Controllers;
using SFA.DAS.TrainingTypes.Application.Application.Commands.DeleteWorkHistory;
using System.Net;

namespace SFA.DAS.TrainingTypes.Api.UnitTests.Controllers.WorkHistory
{
    public class WhenCallingDeleteWorkHistory
    {
        [Test, MoqAutoData]
        public async Task Then_If_MediatorCall_Returns_Ok_Then_Ok_Result_Returned(
            Guid candidateId,
            Guid applicationId,
            Guid id,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] WorkHistoryController controller)
        {
            var actual = await controller.DeleteWorkHistory(candidateId, applicationId, id) as OkObjectResult;

            actual.Should().BeOfType<OkObjectResult>();
            mediator.Verify(x => x.Send(It.Is<DeleteWorkHistoryCommand>(c =>
                    c.CandidateId.Equals(candidateId) &&
                    c.ApplicationId.Equals(applicationId) &&
                    c.JobId.Equals(id)
                ), CancellationToken.None));

        }

        [Test, MoqAutoData]
        public async Task Then_If_Error_Then_InternalServerError_Response_Returned(
            Guid candidateId,
            Guid applicationId,
            Guid id,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] WorkHistoryController controller)
        {
            // Arrange
            mediator.Setup(x => x.Send(It.IsAny<DeleteWorkHistoryCommand>(), CancellationToken.None))
                .ThrowsAsync(new Exception("Error"));

            // Act
            var actual = await controller.DeleteWorkHistory(candidateId, applicationId, id);

            // Assert
            actual.Should().BeOfType<StatusCodeResult>();
            var result = actual as StatusCodeResult;
            result?.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }



    }
}
