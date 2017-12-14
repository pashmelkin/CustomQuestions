using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CustomQuestions.Framework;
using MediatR;
using CustomQuestions.Models;
using Microsoft.AspNetCore.Mvc;


namespace CustomQuestions.Controllers
{
    public class PostCustomQuestionQuery : IRequest<PostCustomQuestionResponse>
    {
        public readonly string _questionText;
        public readonly string _classification;
        public readonly string _subClassification;
      

        public PostCustomQuestionQuery(PostCustomQuestionRequest request)
        {
            this._questionText = request.questionText;
            this._classification = request.classification;
            this._subClassification = request.subClassification;
        }
    }

    public class PostCustomQuestionQueryHandler : IRequestHandler<PostCustomQuestionQuery, PostCustomQuestionResponse>
    {
        private readonly ITokenclient _tokenClient;
        private static List<Question> questionsStatic = new List<Question>();
        private static Dictionary<string, List<string>> tokensRecomQuestionDict = new Dictionary<string, List<string>>();

        public PostCustomQuestionQueryHandler(ITokenclient tokenClient)
        {
            this._tokenClient = tokenClient;
        }

        public async Task<PostCustomQuestionResponse> Handle(PostCustomQuestionQuery request, CancellationToken cancellationToken)
        {
            var tokensCustomQuestion = await this._tokenClient.GetTokensAsync(request._questionText);
            if (!tokensCustomQuestion.Any())
            {
                return new PostCustomQuestionResponse();
            }

            var questions = await TryGetQuestions();            

            var tokenCount = new Hashtable();
            var currChampTokenCount = 0;
            var tokensRecomQuestion = new List<string>();

            foreach (var question in questions)
            {
                if (tokensRecomQuestionDict.ContainsKey(question.ID))
                {
                    tokensRecomQuestionDict.TryGetValue(question.ID, out tokensRecomQuestion);
                }
                else
                {
                    tokensRecomQuestion = await this._tokenClient.GetTokensAsync(question.Text);
                    tokensRecomQuestionDict.Add(question.ID, tokensRecomQuestion);
                }
                
                var commonTokens = tokensCustomQuestion.Intersect(tokensRecomQuestion).ToList();

                if (commonTokens.Count < currChampTokenCount) continue;

                if (commonTokens.Count > currChampTokenCount)
                {
                    // new value
                    currChampTokenCount = commonTokens.Count;
                    tokenCount.Clear();
                }
                tokenCount.Add(question, commonTokens.Count);
            }

            return new PostCustomQuestionResponse
            {
                tokenCount = currChampTokenCount,
                questions = tokenCount.Keys.Cast<Question>().ToList()
            };
        }

        private async Task<List<Question>> TryGetQuestions()
        {

            if (questionsStatic.Any())
            {
                return questionsStatic;
            }
            questionsStatic = await this._tokenClient.GetQuestionsAsync(2000);
                
            return questionsStatic;
        }
    }
}