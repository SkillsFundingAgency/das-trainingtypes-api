using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.TrainingTypes.Api.Controllers;
using SFA.DAS.TrainingTypes.Application.Candidate.Queries.GetSavedVacancy;
using System.Net;

namespace SFA.DAS.TrainingTypes.Api.UnitTests.Controllers.SavedVacancies
{
    [TestFixture]
    public class WhenCallingGetByVacancyReference
    {
        [Test, MoqAutoData]
        public async Task Then_The_Response_Is_Returned_As_Expected(
            Guid candidateId,
            string vacancyReference,
            GetSavedVacancyQueryResult queryResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] SavedVacancyController controller)
        {
            queryResult.VacancyReference = vacancyReference;
            queryResult.CandidateId = candidateId;

            mediator.Setup(x => x.Send(It.Is<GetSavedVacancyQuery>(c => c.CandidateId == candidateId && c.VacancyReference == vacancyReference), It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResult);

            var result = await controller.GetByVacancyReference(candidateId, vacancyReference) as OkObjectResult;

            result.Value.Should().BeEquivalentTo(queryResult);
        }

        [Test, MoqAutoData]
        public async Task Then_The_Response_Is_Null_Returned_As_NotFound(
            Guid candidateId,
            string vacancyReference,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] SavedVacancyController controller)
        {

            mediator.Setup(x => x.Send(It.Is<GetSavedVacancyQuery>(c => c.CandidateId == candidateId && c.VacancyReference == vacancyReference), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetSavedVacancyQueryResult());

            var result = await controller.GetByVacancyReference(candidateId, vacancyReference) as StatusCodeResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Test, MoqAutoData]
        public async Task Then_The_Response_Is_Exception_Returned_As_InternalServerException(
            Guid candidateId,
            string vacancyReference,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] SavedVacancyController controller)
        {

            mediator.Setup(x => x.Send(It.Is<GetSavedVacancyQuery>(c => c.CandidateId == candidateId && c.VacancyReference == vacancyReference), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());

            var result = await controller.GetByVacancyReference(candidateId, vacancyReference) as StatusCodeResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}
