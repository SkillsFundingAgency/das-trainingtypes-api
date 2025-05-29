using Microsoft.AspNetCore.Mvc;
using System.Net;
using MediatR;
using SFA.DAS.TrainingTypes.Api.ApiResponses;
using SFA.DAS.TrainingTypes.Application.Application.Queries.GetLearnerAge;
using SFA.DAS.TrainingTypes.Application.Application.Queries.GetRecognitionOfPriorLearning;
using SFA.DAS.TrainingTypes.Application.Application.Queries.GetTrainingDuration;

namespace SFA.DAS.TrainingTypes.Api.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    public class FeaturesController(IMediator mediator, ILogger<FeaturesController> logger) : Controller
    {
        [HttpGet]
        [Route("api/trainingtypes/{trainingTypeShortCode}/features/rpl")]
        public async Task<IActionResult> GetRecognitionOfPriorLearning([FromRoute] string trainingTypeShortCode)
        {
            try
            {
                var result = await mediator.Send(new GetRecognitionOfPriorLearningQuery
                {
                    TrainingTypeShortCode = trainingTypeShortCode
                });
                return Ok((GetRecognitionOfPriorLearningApiResponse)result);
            }
            catch (Exception e)
            {
                logger.LogError(e, "GetRecognitionOfPriorLearning : An error occurred");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("api/trainingtypes/{trainingTypeShortCode}/features/learnerAge")]
        public async Task<IActionResult> GetLearnerAge([FromRoute] string trainingTypeShortCode)
        {
            try
            {
                var result = await mediator.Send(new GetLearnerAgeQuery
                {
                    TrainingTypeShortCode = trainingTypeShortCode
                });
                return Ok((GetLearnerAgeApiResponse)result);
            }
            catch (Exception e)
            {
                logger.LogError(e, "GetLearnerAge : An error occurred");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("api/trainingtypes/{trainingTypeShortCode}/features/trainingDuration")]
        public async Task<IActionResult> GetTrainingDuration([FromRoute] string trainingTypeShortCode)
        {
            try
            {
                var result = await mediator.Send(new GetTrainingDurationQuery
                {
                    TrainingTypeShortCode = trainingTypeShortCode
                });
                return Ok((GetTrainingDurationApiResponse)result);
            }
            catch (Exception e)
            {
                logger.LogError(e, "GetTrainingDuration : An error occurred");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
