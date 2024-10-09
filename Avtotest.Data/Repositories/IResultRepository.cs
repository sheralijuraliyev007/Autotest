using Avtotest.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avtotest.Data.Repositories
{
    public interface IResultRepository
    {
        public Task<List<Result>> GetAllResults();

        public Task AddResult(byte ticketId, string userId, int correctAnswerCount);

        public Task DeleteResult(Result result);

        public Task<Result?> GetResultById(byte ticketId, string userId);


    }
}
