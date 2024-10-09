using Avtotest.Data.Entities.TestEntities;
using Avtotest.Data.Repositories;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Avtotest.Services.Services
{
    public class TestService
    {
        private readonly TestRepository _repository;


        public List<Test> Tests { get; set; }
        public TestService(TestRepository repository){
            _repository = repository;
            Tests = repository.ReadFromFile();
        }

        public void ChangeLanguage(string language, HttpContext context)
        {
            if (!string.IsNullOrEmpty(language))
            {

                AddCookie("language", language, context);
            }
            else
            {
                language = GetCookie("language", context);
            }



            Tests = _repository.ReadFromFile(language);
        }


        public Tuple<Test, List<Test>> GetTestAndTests(ushort startIndex,ushort endIndex, int testId)
        {
            var tests = Tests.
                Where(t => t.Id >= startIndex && t.Id <= endIndex).ToList();


            var test = tests.Find(t => t.Id == testId);

            return new (test, tests);
        }

        public string GetCookie(string key, HttpContext context)
        {
            string value = context.Request.Cookies[key];
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }
            return value;
        }



        public void AddCookie(string key, string value, HttpContext context)
        {
            var check = CheckCookie(key, context);
            if (!check)
            {
                context.Response.Cookies.Delete(key);
            }

            context.Response.Cookies.Append(key, value);



        }

        public Tuple<Ticket, int> GetTicketAndTestId(int testId, byte ticketId)
        {
            var ticket = new Ticket() { Id = ticketId };



            if (testId == 0)
            {
                testId = ticket.StartIndex;
            }
            var (test, tests) = GetTestAndTests(ticket.StartIndex, ticket.EndIndex, testId);

            return new (ticket,testId);
        }


        public bool CheckCookie(string key, HttpContext context)
        {
            var value = context.Request.Cookies[key];
            return string.IsNullOrEmpty(value);
        }


    }
}
