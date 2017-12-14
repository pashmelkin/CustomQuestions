using Npgsql;
using System.Collections.Generic;
using CustomQuestions.Framework;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Dapper;
using System.Linq;
using CustomQuestions.Models;

namespace CustomQuestions.Domain
{
    public interface IQuestionRepository
    {
        Task<List<Question>> GetQuestions(int batchSize);
    }

    public class QuestionRepository : IQuestionRepository
    {
        private IOptions<PostgreSQLSettings> settings { get; }

        public QuestionRepository(IOptions<PostgreSQLSettings> settings)
        {
            this.settings = settings;
        }
        public async Task<List<Question>> GetQuestions(int batchSize)
        {
            const string sqlQuery = "select \"ID\", \"Text\" from question limit  @batch;";
            List<Question> results = null;
            using (var connection = new NpgsqlConnection(this.settings.Value.ToConnectionString()))
            {
                await connection.OpenAsync();
                results = (await connection.QueryAsync<Question>(sqlQuery, new { batch = batchSize} )).ToList();
                connection.Close();
            }
            return results;
        }
        public async Task<Question> GetQuestionById(string questionId)
        {
            const string sqlQuery = "select \"ID\", \"Text\" from question where \"ID\" = \'@questionId\';";
            Question results = null;
            using (var connection = new NpgsqlConnection(this.settings.Value.ToConnectionString()))
            {
                await connection.OpenAsync();
                results = (await connection.QueryAsync<Question>(sqlQuery, new { questionId })).FirstOrDefault();
                connection.Close();
            }
            return results;
        }
    }
}
