using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Routing;

namespace CustomQuestions.Controllers
{
    [Produces("application/json")]
    [Route("api/Question")]
    public class QuestionController : Controller
    {

        private readonly IMediator _mediator;
        public QuestionController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        // GET: api/Question/batchSize
      
        [HttpGet("{batchSize}")]
        public async Task<ActionResult> GetQuestions(int batchSize)
        {
            var questions = await this._mediator.Send(new GetQuestionsQuery(batchSize));
            if (questions == null)
            {
                return NotFound();
            }

            return Ok(questions);
        }
      
        [HttpGet, Route("Tokenize")]
        public List<string> TokenizeQuestion(string questionText)
        {
            // TODO : Sanitize the question text: remove spaces, 's, geo names etc
            var tokens = questionText.ToLower().Split().ToList();
            return tokens;
        }

    }
}
