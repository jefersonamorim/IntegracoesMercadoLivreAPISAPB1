using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracoesML.Entity
{
        public class Notes
        {
            public string order_id { get; set; }
            public Result[] results { get; set; }
        }

        public class Result
        {
            public string id { get; set; }
            public DateTime date_created { get; set; }
            public DateTime date_last_updated { get; set; }
            public string note { get; set; }
        }

}
