using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracoesML.Entity
{
    class Handle
    {
        string[] handle;

        public string[] insertHandle(string _handle)
        {
            handle = new string[1] { _handle };
            
            return handle; 
        }
    }


}
