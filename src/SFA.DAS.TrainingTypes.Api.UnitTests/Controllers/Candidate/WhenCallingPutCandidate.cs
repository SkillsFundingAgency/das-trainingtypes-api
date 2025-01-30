using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.TrainingTypes.Api.ApiRequests;
using SFA.DAS.TrainingTypes.Api.Controllers;
using SFA.DAS.TrainingTypes.Application.Candidate.Commands.UpsertCandidate;

namespace SFA.DAS.TrainingTypes.Api.UnitTests.Controllers.Candidate;

public class WhenCallingPutCandidate
{
    [Test, MoqAutoData]
    public async Task Then_If_MediatorCall_Returns_Created_Then_Created_Result_Returned(
        Guid id,
        PutCandidateRequest postCandidateRequest,
        UpsertCandidateCommandResponse upsertCandidateCommandResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] CandidateController controller)
    {
        //Arrange
        upsertCandidateCommandResult.IsCreated = true;
        mediator.Setup(x => x.Send(It.Is<UpsertCandidateCommand>(c =>
                c.Candidate.Email.Equals(postCandidateRequest.Email)
                && c.Candidate.Id == id
                && c.Candidate.FirstName.Equals(postCandidateRequest.FirstName)
                && c.Candidate.LastName.Equals(postCandidateRequest.LastName)
                && c.Candidate.MiddleNames.Equals(postCandidateRequest.MiddleNames)
                && c.Candidate.PhoneNumber.Equals(postCandidateRequest.PhoneNumber)
                && c.Candidate.DateOfBirth.Equals(postCandidateRequest.DateOfBirth)
                && c.Candidate.TermsOfUseAcceptedOn.Equals(postCandidateRequest.TermsOfUseAcceptedOn)
            ), CancellationToken.None))
            .ReturnsAsync(upsertCandidateCommandResult);

        //Act
        var actual = await controller.PutCandidate(id, postCandidateRequest);

        //Assert
        var result = actual as CreatedResult;
        var actualResult = result.Value as Domain.Candidate.Candidate;
        actualResult.Should().BeEquivalentTo(upsertCandidateCommandResult.Candidate);
    }

    [Test, MoqAutoData]
    public async Task Then_If_MediatorCall_Returns_NotCreated_Then_Ok_Result_Returned(
        Guid id,
        PutCandidateRequest postCandidateRequest,
        UpsertCandidateCommandResponse upsertCandidateCommandResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] CandidateController controller)
    {
        //Arrange
        upsertCandidateCommandResult.IsCreated = false;
        mediator.Setup(x => x.Send(It.Is<UpsertCandidateCommand>(c =>
                c.Candidate.Email.Equals(postCandidateRequest.Email)
                && c.Candidate.Id.Equals(id)
                && c.Candidate.FirstName.Equals(postCandidateRequest.FirstName)
                && c.Candidate.LastName.Equals(postCandidateRequest.LastName)
                && c.Candidate.MiddleNames.Equals(postCandidateRequest.MiddleNames)
                && c.Candidate.PhoneNumber.Equals(postCandidateRequest.PhoneNumber)
                && c.Candidate.DateOfBirth.Equals(postCandidateRequest.DateOfBirth)
                && c.Candidate.TermsOfUseAcceptedOn.Equals(postCandidateRequest.TermsOfUseAcceptedOn)
                ), CancellationToken.None))
            .ReturnsAsync(upsertCandidateCommandResult);

        //Act
        var actual = await controller.PutCandidate(id, postCandidateRequest);

        //Assert
        var result = actual as OkObjectResult;
        var actualResult = result.Value as Domain.Candidate.Candidate;
        actualResult.Should().BeEquivalentTo(upsertCandidateCommandResult.Candidate);
    }
}