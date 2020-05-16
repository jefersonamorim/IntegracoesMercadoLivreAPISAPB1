using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracoesML.Entity
{
    public class RetNFResponse
    {
        public int status { get; set; }
        public string message { get; set; }
        public string error { get; set; }
        //public object cause { get; set; }
        //public object[] internal_cause { get; set; }
    }

}
