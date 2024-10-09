using Avtotest.Data.Context;
using Avtotest.Data.Entities;
using Avtotest.Data.Entities.TestEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avtotest.Data.Repositories
{
    public class ResultRepository : IResultRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMemoryCache _memoryCache;
        private const string ResultKey = "results";

        public ResultRepository(AppDbContext appDbContext,IMemoryCache memoryCache)
        {
            _appDbContext = appDbContext;
            _memoryCache = memoryCache;
        }

        public async Task<List<Result>> GetAllResults()
        {
            var results = await GetResults();
            return results;
        }

        public async Task AddResult(byte ticketId, string userId, int correctAnswerCount)
        {
            var result = new Result()
            {
                TicketId = ticketId,
                CorrectAnswerCount = (byte)(correctAnswerCount),
                UserId = userId
            };


            _appDbContext.Results.Add(result);
            await _appDbContext.SaveChangesAsync();
            await GetOrUpdateResults();
        }

        public async Task DeleteResult(Result result)
        {
            _appDbContext.Results.Remove(result);
            await _appDbContext.SaveChangesAsync();
            await GetOrUpdateResults();
        }



        private async Task<List<Result>> GetOrUpdateResults()
        {
            var results =await _appDbContext.Results.ToListAsync();
            _memoryCache.Set(ResultKey,results);
            return results;

        }

        public async Task<List<Result>> GetResults()
        {
            if(_memoryCache.TryGetValue(ResultKey, out object value))
            {
                var results=(List<Result>) value!;

                return results;
            }

            return await GetOrUpdateResults();
        }

        public async Task<Result?> GetResultById(byte ticketId, string userId)
        {
            var results= await GetResults();
            return results.FirstOrDefault(r => Convert.ToString(r.Id) == userId && r.TicketId == ticketId);
        }
    }
}
