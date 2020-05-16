using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracoesML.Entity
{
    public class Feed
    {
        public string eventId { get; set; }
        public string handle { get; set; }
        public string domain { get; set; }
        public string state { get; set; }
        public string orderId { get; set; }
        public DateTime lastChange { get; set; }
    }
}
