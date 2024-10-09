using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avtotest.Data.Entities.TestEntities
{
    public class Ticket
    {

        public byte Id { get; set; }
        public ushort StartIndex => (ushort)((Id - 1) * 20 + 1);

        public ushort EndIndex => (ushort)(Id * 20);

    }
}
