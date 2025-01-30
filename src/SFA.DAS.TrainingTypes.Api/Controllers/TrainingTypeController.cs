using Microsoft.AspNetCore.Mvc;
using System.Net;
using MediatR;
using SFA.DAS.TrainingTypes.Api.ApiResponses;

namespace SFA.DAS.TrainingTypes.Api.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/trainingtypes/{shortCode}/features/{featureType}")]
    public class FeaturesController(IMediator mediator, ILogger<FeaturesController> logger) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> GetFeature([FromRoute] string trainingTypeShortCode, [FromRoute] string featureType)
        {
            try
            {
                var result = await mediator.Send(new GetApplicationWorkHistoriesQuery
                {
                    CandidateId = candidateId,
                    ApplicationId = applicationId,
                    WorkHistoryType = workHistoryType
                });
                return Ok((GetFeatureApiResponse)result);
            }
            catch (Exception e)
            {
                logger.LogError(e, "GetFeature : An error occurred");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
