using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using SFA.DAS.TrainingTypes.Api.Controllers;
using SFA.DAS.TrainingTypes.Application.Candidate.Queries.GetInactiveCandidates;

namespace SFA.DAS.TrainingTypes.Api.UnitTests.Controllers.Candidate
{
    [TestFixture]
    public class WhenCallingGetCandidatesByActivity
    {
        [Test, MoqAutoData]
        public async Task Then_If_MediatorCall_Returns_Candidates_Then_Ok_Result_Returned(
            DateTime cutOffDateTime,
            GetInactiveCandidatesQueryResult getCandidatesByActivityQueryResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] CandidateController controller)
        {
            //Arrange
            mediator.Setup(x => x.Send(It.Is<GetInactiveCandidatesQuery>(c =>
                    c.CutOffDateTime == cutOffDateTime
                ), CancellationToken.None))
                .ReturnsAsync(getCandidatesByActivityQueryResult);

            //Act
            var actual = await controller.GetInactiveCandidates(cutOffDateTime);

            //Assert
            var result = actual as OkObjectResult;
            var actualResult = result!.Value as GetInactiveCandidatesQueryResult;
            actualResult!.Candidates.Should().BeEquivalentTo(getCandidatesByActivityQueryResult.Candidates);
        }

        [Test, MoqAutoData]
        public async Task Then_If_Error_Then_InternalServerError_Response_Returned(
            DateTime cutOffDateTime,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] CandidateController controller)
        {
            //Arrange
            mediator.Setup(x => x.Send(It.IsAny<GetInactiveCandidatesQuery>(),
                CancellationToken.None)).ThrowsAsync(new Exception("Error"));

            //Act
            var actual = await controller.GetInactiveCandidates(cutOffDateTime);

            //Assert
            var result = actual as StatusCodeResult;
            result?.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}
