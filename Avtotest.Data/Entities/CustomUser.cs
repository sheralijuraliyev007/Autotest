using Avtotest.Data.Entities.TestEntities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avtotest.Data.Entities
{
    public class CustomUser
    {

        [Key]
              
        
        public int Id {  get; set; }
        public string? PhotoUrl { get; set; }

        public string UserId { get; set; }

        public DateTime CreatedAt { get; set; }= DateTime.Now;

        public List<Result>? Results{ get; set; }


    }
}
