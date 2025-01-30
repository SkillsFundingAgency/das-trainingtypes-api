using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.TrainingTypes.Api.Controllers;
using SFA.DAS.TrainingTypes.Application.Candidate.Commands.DeleteCandidate;
using System.Net;

namespace SFA.DAS.TrainingTypes.Api.UnitTests.Controllers.Candidate
{
    [TestFixture]
    public class WhenDeletingCandidate
    {
        [Test, MoqAutoData]
        public async Task Then_If_MediatorCall_Returns_Candidate_Then_NoContent_Result_Returned(
           Guid id,
           DeleteCandidateCommandResult deleteCandidateCommandResult,
           [Frozen] Mock<IMediator> mediator,
           [Greedy] CandidateController controller)
        {
            //Arrange
            mediator.Setup(x => x.Send(It.Is<DeleteCandidateCommand>(c =>
                    c.CandidateId == id
                ), CancellationToken.None))
                .ReturnsAsync(deleteCandidateCommandResult);

            //Act
            var actual = await controller.DeleteCandidate(id);

            //Assert
            var result = actual as NoContentResult;
            result.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
        }

        [Test, MoqAutoData]
        public async Task Then_If_MediatorCall_Returns_Null_Then_NotFound_Result_Returned(
            Guid id,
            DeleteCandidateCommandResult deleteCandidateCommandResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] CandidateController controller)
        {
            //Arrange
            deleteCandidateCommandResult.Candidate = null;
            mediator.Setup(x => x.Send(It.Is<DeleteCandidateCommand>(c =>
                    c.CandidateId == id
                ), CancellationToken.None))
                .ReturnsAsync(deleteCandidateCommandResult);

            //Act
            var actual = await controller.DeleteCandidate(id);

            //Assert
            var result = actual as NoContentResult;
            result.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
        }

        [Test, MoqAutoData]
        public async Task Then_If_Error_Then_InternalServerError_Response_Returned(
            Guid id,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] CandidateController controller)
        {
            //Arrange
            mediator.Setup(x => x.Send(It.IsAny<DeleteCandidateCommand>(),
                CancellationToken.None)).ThrowsAsync(new Exception("Error"));

            //Act
            var actual = await controller.DeleteCandidate(id);

            //Assert
            var result = actual as StatusCodeResult;
            result?.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}
