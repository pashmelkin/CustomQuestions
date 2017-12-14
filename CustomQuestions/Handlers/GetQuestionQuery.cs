using System.Threading.Tasks;
using MediatR;
using CustomQuestions.Models;
using CustomQuestions.Domain;
using System.Collections.Generic;
using System.Threading;

namespace CustomQuestions.Controllers
{
    public class GetQuestionsQuery : IRequest<List<Question>>
    {
        public int batchSize;

        public GetQuestionsQuery(int batchSize)
        {
            this.batchSize = batchSize;
        }
    }

    public class GetQuestionsQueryHandler : IRequestHandler<GetQuestionsQuery, List<Question>>
    {

        private IQuestionRepository repository;

        public GetQuestionsQueryHandler(IQuestionRepository repository)
        {
            this.repository = repository;
        }

        public async Task<List<Question>> Handle(GetQuestionsQuery request, CancellationToken cancellationToken)
        {
            var results = await this.repository.GetQuestions(request.batchSize);
            return results;
        }
    }
}