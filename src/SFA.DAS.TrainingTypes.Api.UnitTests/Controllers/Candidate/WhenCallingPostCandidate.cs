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
using SFA.DAS.TrainingTypes.Application.Candidate.Commands.CreateCandidate;

namespace SFA.DAS.TrainingTypes.Api.UnitTests.Controllers.Candidate;

public class WhenCallingPostCandidate
{
    [Test, MoqAutoData]
    public async Task Then_If_MediatorCall_Returns_Created_Then_Created_Result_Returned(
        string id,
        PostCandidateRequest postCandidateRequest,
        CreateCandidateCommandResponse createCandidateCommandResponse,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] CandidateController controller)
    {
        //Arrange
        mediator.Setup(x => x.Send(It.Is<CreateCandidateCommand>(c =>
                c.Email.Equals(postCandidateRequest.Email)
                && c.GovUkIdentifier.Equals(id.ToString())
                && c.FirstName.Equals(postCandidateRequest.FirstName)
                && c.LastName.Equals(postCandidateRequest.LastName)
                && c.DateOfBirth.Equals(postCandidateRequest.DateOfBirth)
                && c.MigratedCandidateId.Equals(postCandidateRequest.MigratedCandidateId)
                && c.MigratedEmail.Equals(postCandidateRequest.MigratedEmail)
                && c.PhoneNumber.Equals(postCandidateRequest.PhoneNumber)
            ), CancellationToken.None))
            .ReturnsAsync(createCandidateCommandResponse);

        //Act
        var actual = await controller.PostCandidate(id, postCandidateRequest);

        //Assert
        var result = actual as CreatedResult;
        var actualResult = result.Value as Domain.Candidate.Candidate;
        actualResult.Should().BeEquivalentTo(createCandidateCommandResponse.Candidate);
    }
    [Test, MoqAutoData]
    public async Task Then_If_Error_Then_InternalServerError_Response_Returned(
        string id,
        PostCandidateRequest postCandidateRequest,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] CandidateController controller)
    {
        //Arrange
        mediator.Setup(x => x.Send(It.IsAny<UpsertApplicationCommand>(),
            CancellationToken.None)).ThrowsAsync(new Exception("Error"));

        //Act
        var actual = await controller.PostCandidate(id, postCandidateRequest);

        //Assert
        var result = actual as StatusCodeResult;
        result?.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}