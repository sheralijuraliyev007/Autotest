using Avtotest.Data.Context;
using Avtotest.Data.Entities;
using Avtotest.Data.Entities.TestEntities;
using Avtotest.Data.Repositories;
using Avtotest.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net.Sockets;
using static System.Net.Mime.MediaTypeNames;

namespace Avtotest.MVC.Controllers
{

    
    public class TestController : Controller
    {
        private readonly TestService _testService;
        private readonly UserManager<IdentityUser>  _userManager;
        private readonly IResultRepository _resultRepository;

        private const string CorrectAnswersCount = "CorrectAnswersCount";

        public TestController(TestService testService, UserManager<IdentityUser> userManager,AppDbContext appDbContext, IResultRepository resultRepository)
        {
            _testService = testService;
            _userManager = userManager;
            _resultRepository = resultRepository;
        }
        [Authorize]
        public async Task<IActionResult> GetTests(byte ticketId,int testId=0,string language=null,bool isRetaken=false)
        {
            var user = await GetUser();

            var result =await _resultRepository.GetResultById(ticketId,user.Id);


            

            if(result is not null  && !isRetaken)
            {
                return RedirectToAction("Results",result);
            }
            if (isRetaken && result is not null)
            {
                await _resultRepository.DeleteResult(result);
                //_appDbContext.Results.Remove(result);
            }
            

            
            


            _testService.ChangeLanguage(language,HttpContext);

            (Ticket ticket, testId)= _testService.GetTicketAndTestId(testId, ticketId);


            ViewBag.TikcetId=ticket.Id;

            ViewBag.Context = HttpContext;

            ViewBag.Ticket=ticket;

            var (test, tests) = _testService.GetTestAndTests(ticket.StartIndex, ticket.EndIndex, testId);

            ViewBag.Tests=tests;


            return View(test);
        }



        [HttpPost]
        public async Task<IActionResult> GetTestsPost(byte ticketId =0 , int testId = 0 ,int choiceId=0)
        {
            int count = GetCorrectAnswersCount();
            var ticket = new Ticket()
            {
                Id = ticketId,
            };
            


            var test = _testService.Tests.Find(t => t.Id == testId);


            await AddScore(test, choiceId, testId, count);
            
            return RedirectToAction("GetTests", new {ticketId=ticketId , testId=test.Id});
        }

        public async Task<IActionResult> TestResult(byte ticketId)
        {
            var correctAnswerCount = GetCorrectAnswersCount();
            ViewBag.Count = correctAnswerCount;
            var ticket = new Ticket() { Id = ticketId };

            var user = await GetUser();

            await _resultRepository.AddResult(ticketId, user.Id, correctAnswerCount);
            DeleteCookies(ticket);
            DeleteCookie("language");
            return View();
        }



        public async Task<IActionResult> Results(Result result)
        {
            return View(result);
        }

        public IActionResult Tickets()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Tickets(byte id)
        {
            var ticket = new Ticket() { Id = id };

            DeleteCookies(ticket); 


            return RedirectToAction("GetTests", new { ticketId = id, testId = 0 });
        }

        private void DeleteCookies(Ticket ticket)
        {
            for (int i = ticket.StartIndex; i <= ticket.EndIndex; i++)
            {
                DeleteCookie(i.ToString());
                if (i == ticket.StartIndex)
                {
                    DeleteCookie(CorrectAnswersCount);
                }
            }
        }




        [Authorize]



        private void DeleteCookie(string key) {
            var check = _testService.CheckCookie(key,HttpContext);
            if (!check)
            {
                HttpContext.Response.Cookies.Delete(key);
            }
        }

        

        private int GetCorrectAnswersCount()
        {
            string correctAnswerCount = HttpContext.Request.Cookies[CorrectAnswersCount];
            int count;
            if (string.IsNullOrEmpty(correctAnswerCount))
            {
                count = 0;
            }
            else
            {
                
                count = Convert.ToInt32(correctAnswerCount);
            }
            return count;
        }

        private async Task<IdentityUser> GetUser()
        {
            return await _userManager.GetUserAsync(User);
        }

        private async Task AddScore(Test test,int choiceId,int testId, int count)
        {
            if (test.Choices[choiceId].Answer)
            {
                count++;
            }
            if (testId != 0)
            {

                _testService.AddCookie(test.Id.ToString(), choiceId.ToString(), HttpContext);
                _testService.AddCookie(CorrectAnswersCount, count.ToString(), HttpContext);
            }

        }

    }
}
