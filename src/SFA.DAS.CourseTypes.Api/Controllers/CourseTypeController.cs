using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.CourseTypes.Api.ApiResponses;
using SFA.DAS.CourseTypes.Application.Application.Queries.GetCourseDuration;
using SFA.DAS.CourseTypes.Application.Application.Queries.GetLearnerAge;
using SFA.DAS.CourseTypes.Application.Application.Queries.GetRecognitionOfPriorLearning;

namespace SFA.DAS.CourseTypes.Api.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    public class FeaturesController(IMediator mediator, ILogger<FeaturesController> logger) : Controller
    {
        [HttpGet]
        [Route("api/coursetypes/{courseTypeShortCode}/features/rpl")]
        public async Task<IActionResult> GetRecognitionOfPriorLearning([FromRoute] string courseTypeShortCode)
        {
            try
            {
                var result = await mediator.Send(new GetRecognitionOfPriorLearningQuery
                {
                    CourseTypeShortCode = courseTypeShortCode
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
        [Route("api/coursetypes/{courseTypeShortCode}/features/learnerAge")]
        public async Task<IActionResult> GetLearnerAge([FromRoute] string courseTypeShortCode)
        {
            try
            {
                var result = await mediator.Send(new GetLearnerAgeQuery
                {
                    CourseTypeShortCode = courseTypeShortCode
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
        [Route("api/coursetypes/{courseTypeShortCode}/features/courseDuration")]
        public async Task<IActionResult> GetCourseDuration([FromRoute] string courseTypeShortCode)
        {
            try
            {
                var result = await mediator.Send(new GetCourseDurationQuery
                {
                    CourseTypeShortCode = courseTypeShortCode
                });
                return Ok((GetCourseDurationApiResponse)result);
            }
            catch (Exception e)
            {
                logger.LogError(e, "GetCourseDuration : An error occurred");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
