using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using SFA.DAS.TrainingTypes.Api.Controllers;
using SFA.DAS.TrainingTypes.Application.Candidate.Queries.GetSavedVacancy;
using SFA.DAS.TrainingTypes.Application.Candidate.Commands.DeleteSavedVacancy;

namespace SFA.DAS.TrainingTypes.Api.UnitTests.Controllers.SavedVacancies
{
    [TestFixture]
    public class WhenCallingDeleteSavedVacancy
    {
        [Test, MoqAutoData]
        public async Task Then_The_Response_Is_Returned_As_Expected(
            Guid candidateId,
            string vacancyReference,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] SavedVacancyController controller)
        {
            var result = await controller.DeleteSavedVacancy(candidateId, vacancyReference) as NoContentResult;

            result.Should().BeOfType<NoContentResult>();

            mediator.Verify(x => x.Send(It.Is<DeleteSavedVacancyCommand>(c =>
                c.CandidateId.Equals(candidateId) &&
                c.VacancyReference.Equals(vacancyReference)
            ), CancellationToken.None));
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

            var actual = await controller.GetByVacancyReference(candidateId, vacancyReference);

            actual.Should().BeOfType<StatusCodeResult>();
            var result = actual as StatusCodeResult;
            result?.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}