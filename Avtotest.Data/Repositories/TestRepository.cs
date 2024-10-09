using Avtotest.Data.Entities.TestEntities;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avtotest.Data.Repositories
{
    public class TestRepository
    {
        private readonly IHostingEnvironment _environment;
        private string Path=> _environment.WebRootPath+"\\Autotest\\";

        public TestRepository(IHostingEnvironment environment)
        {
            _environment = environment;
        }

        //private string Path { get; set; } = "D:\\.net_projects\\AvtotestWeb\\Avtotest.Data\\bin\\Debug\\net8.0\\";
        public List<Test> ReadFromFile(string language = null)
        {

            string filePath = Path;

            if (string.IsNullOrEmpty(language))
            {
                language = "uzb";
            }


            switch (language)
            {
                case "uzb": filePath += "uzlotin.json"; break;
                case "ru": filePath += "rus.json"; break;
                case "kiril": filePath += "uzkiril.json"; break;
            }

            var jsonData = File.ReadAllText(filePath);
            List<Test> tests = JsonConvert.DeserializeObject<List<Test>>(jsonData);
            return tests;
        }
    }
}
