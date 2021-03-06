﻿using System.Threading.Tasks;
using Application.BusinessLogicLayer.Modules.WordsCounter.Queries;
using Application.BusinessLogicLayer.Modules.WordsCounter.RequestModels;
using Application.BusinessLogicLayer.Modules.WordsCounter.ResponseModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Web.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{api-version:apiVersion}/[controller]")]
    public class WordsCounterController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WordsCounterController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("GetBasicData")]
        public async Task<ActionResult> GetBasicData(GetBasicDataRequestModel requestModel)
        {
            GetBasicDataResponseModel responseModel = await _mediator.Send(new GetBasicDataQuery(requestModel));

            return Ok(responseModel);
        }

        [HttpPost("GetTextAnalysisData")]
        public async Task<ActionResult> GetTextAnalysisData(GetTextAnalysisDataRequestModel requestModel)
        {
            GetTextAnalysisDataResponseModel responseModel = await _mediator.Send(new GetTextAnalysisDataQuery(requestModel));

            return Ok(responseModel);
        }

        [HttpPost("GetKeywordDensityData")]
        public async Task<ActionResult> GetKeywordDensityData(GetKeywordDensityDataRequestModel requestModel)
        {
            GetKeywordDensityDataResponseModel responseModel = await _mediator.Send(new GetKeywordDensityDataQuery(requestModel));

            return Ok(responseModel);
        }
    }
}
