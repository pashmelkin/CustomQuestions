using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace CustomQuestions.Controllers
{
    [Route("api/[controller]")]
    public class MainController : Controller
    {

        private readonly IMediator mediator;

        public MainController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost, Route("question")]
        public async Task<ActionResult> Post([FromBody] PostCustomQuestionRequest request)
        {
            if (string.IsNullOrEmpty(request?.questionText))
            {
                return BadRequest("Request doesn't contain the custom question text");
            }

            var questions = await this.mediator.Send(new PostCustomQuestionQuery(request));

            return new OkObjectResult(questions);
        }


    }
}
