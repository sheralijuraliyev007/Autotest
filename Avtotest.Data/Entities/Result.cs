using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avtotest.Data.Entities
{
    public class Result
    {
        [Key]
        public int Id { get; set; }

        public byte TicketId { get; set; }

        public byte CorrectAnswerCount { get; set; }

        public byte IncorrectAnswerCount => (byte)(TotalAnswerCount - CorrectAnswerCount);

        public const byte TotalAnswerCount = 20;

        public string UserId { get; set; }

        public CustomUser? CustomUser { get; set; }

        public DateTime CreatedAt { get; set; }=DateTime.Now;
    }
}
