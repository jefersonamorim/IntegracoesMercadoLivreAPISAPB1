using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracoesML.Entity
{
    public class OrderRecent
    {
        public string query { get; set; }
        public List<Order> results { get; set; }
    }


}
