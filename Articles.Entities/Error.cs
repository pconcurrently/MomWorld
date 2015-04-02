using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomWorld.Entities
{
    public class Error
    {
        public int HttpStatusCode { get; set; }

        public Exception Exception { get; set; }
    }
}
