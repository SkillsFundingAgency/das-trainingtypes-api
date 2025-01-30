using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.CandidateAccount.Application.Candidate.Queries.GetSavedVacancies;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.TrainingTypes.Api.ApiRequests;
using SFA.DAS.TrainingTypes.Api.Controllers;
using SFA.DAS.TrainingTypes.Application.Candidate.Commands.AddSavedVacancy;

namespace SFA.DAS.TrainingTypes.Api.UnitTests.Controllers.SavedVacancies
{
    [TestFixture]
    public class WhenCallingPut
    {
        [Test, MoqAutoData]
        public async Task Then_The_Response_Is_Returned_As_Expected(
            Guid candidateId,
            SavedVacancyRequest request,
            AddSavedVacancyCommandResult commandResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] SavedVacancyController controller)
        {
            mediator.Setup(x => x.Send(It.Is<AddSavedVacancyCommand>(c => c.CandidateId == candidateId && c.VacancyReference == request.VacancyReference), It.IsAny<CancellationToken>()))
                .ReturnsAsync(commandResult);

            var result = await controller.Put(candidateId, request) as OkObjectResult;
            result.Value.Should().BeEquivalentTo(commandResult.SavedVacancy);
        }
    }
}
