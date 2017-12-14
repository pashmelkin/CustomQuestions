using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CustomQuestions.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace CustomQuestions.Framework
{

    public interface ITokenclient
    {
        Task<List<string>> GetTokensAsync(string text);
        Task<List<Question>> GetQuestionsAsync(int batchSize);
    }
    public class TokenClient : ITokenclient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public TokenClient(IHttpClientFactory httpClientFactory)
        {
            //this._httpClient = serviceProvider.GetService<HttpClient>();
            this._httpClientFactory = httpClientFactory;

        }

        public async Task<List<string>> GetTokensAsync(string text)
        {
            var response = await this._httpClientFactory.CreateClient().GetAsync("api/Question/Tokenize?questionText=" + text);
            response.EnsureSuccessStatusCode();
            var tokens = await response.Content.ReadAsStringAsync();
            var tokensList = JsonConvert.DeserializeObject<List<string>>(tokens);
            return tokensList;
        }

        public async Task<List<Question>> GetQuestionsAsync(int batchSize)
        {
            var response = await this._httpClientFactory.CreateClient().GetAsync("api/Question/" + batchSize);
            response.EnsureSuccessStatusCode();
            var questions = await response.Content.ReadAsStringAsync();
            var questionsList = JsonConvert.DeserializeObject<List<Question>>(questions);
            return questionsList;
        }
        public async Task<Question> GetQuestionIDAsync(int questionId)
        {
            var response = await this._httpClientFactory.CreateClient().GetAsync("api/Question/Id/" + questionId);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var question = JsonConvert.DeserializeObject<Question>(content);
            return question;
        }
    }
}
